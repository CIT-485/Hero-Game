using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu]
public class BehaviourTree : ScriptableObject, ISerializationCallbackReceiver
{
    public Node rootNode;
    public Node.State treeState = Node.State.RUNNING;
    public List<Node> nodes = new List<Node>();
    public Blackboard blackboard;
    public Node.State Update()
    {
        if (rootNode.state == Node.State.RUNNING)
            treeState = rootNode.Update();
        return treeState;
    }

#if UNITY_EDITOR
    public Node CreateNode(System.Type type)
    {
        Node node = ScriptableObject.CreateInstance(type) as Node;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        node.blackboard = blackboard;

        Undo.RecordObject(this, "Behaviour Tree (CreateNode)");
        nodes.Add(node);

        if (!Application.isPlaying)
        {
            AssetDatabase.AddObjectToAsset(node, this);
        }
        Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (CreateNode)");

        AssetDatabase.SaveAssets();
        
        return node;
    }
    public void DeleteNode(Node node)
    {
        Undo.RecordObject(this, "Behaviour Tree (DeleteNode)");
        nodes.Remove(node);

        //AssetDatabase.RemoveObjectFromAsset(node);
        Undo.DestroyObjectImmediate(node);

        AssetDatabase.SaveAssets();
    }
    public void AddChild(Node parent, Node child)
    {
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            Undo.RecordObject(decorator, "Behaviour Tree (AddChild)");
            decorator.child = child;
            EditorUtility.SetDirty(decorator);
        }

        RootNode root = parent as RootNode;
        if (root)
        {
            Undo.RecordObject(root, "Behaviour Tree (AddChild)");
            root.child = child;
            EditorUtility.SetDirty(root);
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            Undo.RecordObject(composite, "Behaviour Tree (AddChild)");
            composite.children.Add(child);
            EditorUtility.SetDirty(composite);
        }
    }
    public void RemoveChild(Node parent, Node child)
    {
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            Undo.RecordObject(decorator, "Behaviour Tree (RemoveChild)");
            decorator.child = null;
            EditorUtility.SetDirty(decorator);
        }

        RootNode root = parent as RootNode;
        if (root)
        {
            Undo.RecordObject(root, "Behaviour Tree (RemoveChild)");
            root.child = null;
            EditorUtility.SetDirty(root);
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            Undo.RecordObject(composite, "Behaviour Tree (RemoveChild)");
            composite.children.Remove(child);
            EditorUtility.SetDirty(composite);
        }
    }
#endif
    public List<Node> GetChildren(Node parent)
    {
        List<Node> children = new List<Node>();
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator && decorator.child != null)
        {
            children.Add(decorator.child);
        }

        RootNode root = parent as RootNode;
        if (root && root.child != null)
        {
            children.Add(root.child);
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            return composite.children;
        }
        return children;
    }

    public void Traverse(Node node, System.Action<Node> visitor)
    {
        if (node)
        {
            visitor.Invoke(node);
            var children = GetChildren(node);
            children.ForEach((n) => Traverse(n, visitor));
        }
    }

    public BehaviourTree Clone()
    {
        BehaviourTree tree = Instantiate(this);
        tree.rootNode = rootNode.Clone();
        tree.nodes = new List<Node>();
        Traverse(tree.rootNode, (n) =>
        {
            tree.nodes.Add(n);
        });
        return tree;
    }

    public void Bind()
    {
        Traverse(rootNode, node => 
        {
            node.blackboard = blackboard;
        });
    }

    public void OnBeforeSerialize() { Bind(); }
    public void OnAfterDeserialize() { }
}