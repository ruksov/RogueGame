using System;

namespace Rogue.NodeGraph
{
  [Serializable]
  public struct NodeLink
  {
    public Node First;
    public Node Second;

    public NodeLink(Node first, Node second)
    {
      First = first;
      Second = second;
    }
  }
}