using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class LinkedList : IEnumerable<int>
{
    public Node Head { get; private set; }
    public Node Tail { get; private set; }

    public LinkedList()
    {
        Head = null;
        Tail = null;
    }

    public bool HeadAndTailAreNull()
    {
        return Head == null && Tail == null;
    }

    public bool HeadAndTailAreNotNull()
    {
        return Head != null && Tail != null;
    }

    public void InsertHead(int value)
    {
        Node newNode = new Node(value);
        if (Head == null)
        {
            Head = newNode;
            Tail = newNode;
        }
        else
        {
            newNode.Next = Head;
            Head.Prev = newNode;
            Head = newNode;
        }
    }

    public void InsertAfter(int value, int newValue)
    {
        Node curr = Head;
        while (curr != null)
        {
            if (curr.Data == value)
            {
                Node newNode = new Node(newValue);
                newNode.Next = curr.Next;
                newNode.Prev = curr;

                if (curr.Next == null)
                {
                    Tail = newNode;
                }
                else
                {
                    curr.Next.Prev = newNode;
                }
                curr.Next = newNode;
                return;
            }
            curr = curr.Next;
        }
    }

    public void InsertTail(int value)
    {
        Node newNode = new Node(value);
        if (Tail == null)
        {
            Head = newNode;
            Tail = newNode;
        }
        else
        {
            newNode.Prev = Tail;
            Tail.Next = newNode;
            Tail = newNode;
        }
    }

    public void RemoveTail()
    {
        if (Tail == null) return;

        if (Head == Tail)
        {
            Head = null;
            Tail = null;
        }
        else
        {
            Tail = Tail.Prev;
            Tail.Next = null;
        }
    }

    public void RemoveHead()
    {
        if (Head == null) return;
        if (Head == Tail)
        {
            Head = null;
            Tail = null;
        }
        else
        {
            Head = Head.Next;
            Head.Prev = null;
        }
    }

    public void Remove(int value)
    {
        Node curr = Head;
        while (curr != null)
        {
            if (curr.Data == value)
            {
                if (curr == Head)
                {
                    RemoveHead();
                }
                else if (curr == Tail)
                {
                    RemoveTail();
                }
                else
                {
                    curr.Prev.Next = curr.Next;
                    curr.Next.Prev = curr.Prev;
                }
                return;
            }
            curr = curr.Next;
        }
    }

    public void Replace(int oldValue, int newValue)
    {
        Node curr = Head;
        while (curr != null)
        {
            if (curr.Data == oldValue)
            {
                curr.Data = newValue;
            }
            curr = curr.Next;
        }
    }

    public IEnumerable<int> Reverse()
    {
        Node curr = Tail;
        while (curr != null)
        {
            yield return curr.Data;
            curr = curr.Prev;
        }
    }

    public IEnumerator<int> GetEnumerator()
    {
        Node curr = Head;
        while (curr != null)
        {
            yield return curr.Data;
            curr = curr.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<LinkedList>{");
        Node curr = Head;
        bool first = true;
        while (curr != null)
        {
            if (!first) sb.Append(", ");
            sb.Append(curr.Data);
            first = false;
            curr = curr.Next;
        }
        sb.Append("}");
        return sb.ToString();
    }
}

public static class TestingExtensions
{
    public static string AsString(this IEnumerable<int> collection)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<IEnumerable>{");
        bool first = true;
        foreach (int item in collection)
        {
            if (!first) sb.Append(", ");
            sb.Append(item);
            first = false;
        }
        sb.Append("}");
        return sb.ToString();
    }
}
