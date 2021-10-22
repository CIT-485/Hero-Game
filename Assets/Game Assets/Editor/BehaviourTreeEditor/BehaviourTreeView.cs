using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;
using System;
using System.Linq;

public class BehaviourTreeView : GraphView
{
    public Action<NodeView> OnNodeSelected;
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }
    BehaviourTree tree;

    public BehaviourTreeView()
    {
        Insert(0, new GridBackground());
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Game Assets/Editor/BehaviourTreeEditor/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);

        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnUndoRedo()
    {
        PopulateView(tree);
        AssetDatabase.SaveAssets();
    }

    NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    internal void PopulateView(BehaviourTree tree)
    {
        this.tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements as System.Collections.Generic.IEnumerable<UnityEditor.Experimental.GraphView.GraphElement>);
        graphViewChanged += OnGraphViewChanged;

        if (tree)
        {
            if (tree.rootNode == null)
            {
                tree.nodes.Clear();
                tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
                EditorUtility.SetDirty(tree);
                AssetDatabase.SaveAssets();
            }

            // Create the node views
            tree.nodes.ForEach(n => CreateNodeView(n));

            // Create the edges
            tree.nodes.ForEach(n => 
            {
                var children = tree.GetChildren(n);
                children.ForEach(c => 
                {
                    NodeView parentView = FindNodeView(n);
                    NodeView childView = FindNodeView(c);

                    Edge edge = parentView.output.ConnectTo(childView.input);
                    AddElement(edge);
                });
            });
        }
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort =>
        endPort.direction != startPort.direction &&
        endPort.node != startPort.node).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem => 
            {
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    tree.DeleteNode(nodeView.node);
                }
                Edge edge = elem as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    tree.RemoveChild(parentView.node, childView.node);
                }
            });

        }

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                tree.AddChild(parentView.node, childView.node);
            });
        }

        if (graphViewChange.movedElements != null)
        {
            nodes.ForEach((n) =>
            {
                NodeView view = n as NodeView;
                view.SortChildren();
            });
        }

        UpdateNodeStates();

        return graphViewChange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types)
            {
                if (type.Name == "DelegateNode")
                    evt.menu.AppendAction($"[{type.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }
        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types)
            {
                if (type.Name != "DelegateNode")
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }
        {
            var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }
        {
            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }
    }

    void CreateNode(System.Type type)
    {
        Node node = tree.CreateNode(type);
        CreateNodeView(node);
    }

    void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }

    public void UpdateNodeStates()
    {
        nodes.ForEach((n) =>
        {
            NodeView view = n as NodeView;
            RepeatNode repeater = view.node as RepeatNode;
            view.UpdateState();
        });
        nodes.ForEach((n) =>
        {
            NodeView view = n as NodeView;
            RepeatNode repeater = view.node as RepeatNode;
            if (repeater && repeater.prevCountForState != repeater.count)
            {
                repeater.prevCountForState = repeater.count;
                List<Node> list = new List<Node>();
                Traverse(repeater, node =>
                {
                    list.Add(node);
                });
                nodes.ForEach((m) =>
                {
                    NodeView view2 = m as NodeView;
                    if (list.Contains(view2.node))
                        view2.RemoveStatesFromClassList();
                });
            }
        });
    }

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
}
