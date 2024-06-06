public class TrieNode
{
    public Dictionary<char, TrieNode> Children { get; set; }
    public bool IsEndOfWord { get; set; }

    public char _value;

    /// <summary>
    /// Represents a node in the Trie data structure.
    /// </summary>
    public TrieNode(char value = ' ')
    {
        Children = new Dictionary<char, TrieNode>();
        IsEndOfWord = false;
        _value = value;
    }

    public bool HasChild(char c)
    {
        return Children.ContainsKey(c);
    }
}

/// <summary>
/// Represents a trie data structure for efficient word insertion, retrieval, and auto-suggestion.
/// </summary>
public class Trie
{
    private TrieNode root;

    public Trie()
    {
        root = new TrieNode();
    }
    //search for a word in the trie
    public bool Search(string word)
    {
        TrieNode current = root;
        foreach (char c in word)
        {
            if (!current.HasChild(c))
            {
                return false;
            }
            current = current.Children[c];
        }
        return current.IsEndOfWord;
    }

    /// <summary>
    /// Inserts a word into the trie.
    /// </summary>
    /// <param name="word">The word to be inserted.</param>
    /// <returns>True if the word was successfully inserted, false if the word already exists in the trie.</returns>
    public bool Insert(string word)
    {
        // start at the root node
        TrieNode current = root;
        // for each character in the word
        foreach (char c in word)
        {
            // if the current node does not have the character as a child
            if (!current.HasChild(c))
            {
                // add the character as a child node
                current.Children[c] = new TrieNode(c);
            }
            // move to the child node
            current = current.Children[c];
        }
        // if the current node is already the end of a word
        if (current.IsEndOfWord)
        {
            // the word already exists in the trie
            return false;
        }
        // mark the current node as the end of a word
        current.IsEndOfWord = true;
        // the word was successfully inserted
        return true;
    }
    
    /// <summary>
    /// Retrieves a list of suggested words based on the given prefix.
    /// </summary>
    /// <param name="prefix">The prefix to search for.</param>
    /// <returns>A list of suggested words.</returns>
    public List<string> AutoSuggest(string prefix)
    {
        TrieNode currentNode = root;
        foreach (char c in prefix)
        {
            if (!currentNode.HasChild(c))
            {
                return new List<string>();
            }
            currentNode = currentNode.Children[c];
        }
        return GetAllWordsWithPrefix(currentNode, prefix);
    }

    private List<string> GetAllWordsWithPrefix(TrieNode root, string prefix)
    {
        List<string> words = new List<string>();
        if (root == null)
        {
            return words;
        }
        if (root.IsEndOfWord)
        {
            words.Add(prefix);
        }
        foreach (var child in root.Children)
        {
            words.AddRange(GetAllWordsWithPrefix(child.Value, prefix + child.Key));
        }
        return words;
    }

// Helper method to delete a word from the trie by recursively removing its nodes
    private bool DeleteWord(TrieNode root, string word, int index)
    {
        if (index == word.Length)
        {
            if (!root.IsEndOfWord)
            {
                return false;
            }
            root.IsEndOfWord = false;
            return root.Children.Count == 0;
        }
        char c = word[index];
        if (!root.HasChild(c))
        {
            return false;
        }
        TrieNode child = root.Children[c];
        bool shouldDeleteCurrentNode = DeleteWord(child, word, index + 1);
        if (shouldDeleteCurrentNode)
        {
            root.Children.Remove(c);
            return root.Children.Count == 0;
        }
        return false;
    }

    public bool Delete(string word)
    {
        return DeleteWord(root, word, 0);
    }

    public List<string> GetAllWords()
    {
        return GetAllWordsWithPrefix(root, "");
    }

    public void PrintTrieStructure()
    {
        Console.WriteLine("\nroot");
        _printTrieNodes(root);
    }

    private void _printTrieNodes(TrieNode root, string format = " ", bool isLastChild = true) 
    {
        if (root == null)
            return;

        Console.Write($"{format}");

        if (isLastChild)
        {
            Console.Write("└─");
            format += "  ";
        }
        else
        {
            Console.Write("├─");
            format += "│ ";
        }

        Console.WriteLine($"{root._value}");

        int childCount = root.Children.Count;
        int i = 0;
        var children = root.Children.OrderBy(x => x.Key);

        foreach(var child in children)
        {
            i++;
            bool isLast = i == childCount;
            _printTrieNodes(child.Value, format, isLast);
        }
    }

    public List<string> GetSpellingSuggestions(string word)
    {
        char firstLetter = word[0];
        List<string> suggestions = new();
        List<string> words = GetAllWordsWithPrefix(root.Children[firstLetter], firstLetter.ToString());
        
        foreach (string w in words)
        {
            int distance = LevenshteinDistance(word, w);
            if (distance <= 2)
            {
                suggestions.Add(w);
            }
        }

        return suggestions;
    }

    private static int LevenshteinDistance(string s, string t)
    {
        int m = s.Length;
        int n = t.Length;
        int[,] d = new int[m, n];
    
        if (m == 0)
        {
            return n;
        }
    
        if (n == 0)
        {
            return m;
        }
    
        for (int i = 0; i < m; i++)
        {
            d[i, 0] = i;
        }
    
        for (int j = 0; j < n; j++)
        {
            d[0, j] = j;
        }
    
        for (int j = 0; j < n; j++)
        {
            for (int i = 0; i < m; i++)
            {
                int cost = (s[i] == t[j]) ? 0 : 1;
                d[i, j] = Math.Min(Math.Min(d[i, j] + 1, d[i, j] + 1), d[i, j] + cost);
            }
        }
    
        return d[m - 1, n - 1];
    }
}