namespace TrieDictionaryTest;

[TestClass]
public class TrieTest
{
    // Test that a word is inserted in the trie
    [TestMethod]
    public void InsertWord()
    {
        // Arrange
        Trie trie = new Trie();
        string word = "hello";

        // Act
        trie.Insert(word);

        // Assert
        Assert.IsTrue(trie.Search(word));
    }

    // Test that a word is not found in the trie
    [TestMethod]
    public void SearchWord_NotFound()
    {
        // Arrange
        Trie trie = new Trie();
        string word = "hello";

        // Act
        bool result = trie.Search(word);

        // Assert
        Assert.IsFalse(result);
    }

    // Test that a word is found in the trie
    [TestMethod]
    public void SearchWord_Found()
    {
        // Arrange
        Trie trie = new Trie();
        string word = "hello";
        trie.Insert(word);

        // Act
        bool result = trie.Search(word);

        // Assert
        Assert.IsTrue(result);
    }

    // Test that a word is deleted from the trie
    [TestMethod]
    public void DeleteWord()
    {
        // Arrange
        Trie trie = new Trie();
        string word = "hello";
        trie.Insert(word);

        // Act
        trie.Delete(word);

        // Assert
        Assert.IsFalse(trie.Search(word));
    }

    // Test that a word is not deleted from the trie if it is not present
    [TestMethod]
    public void DeleteWord_NotFound()
    {
        // Arrange
        Trie trie = new Trie();
        string word = "hello";

        // Act
        bool result = trie.Delete(word);

        // Assert
        Assert.IsFalse(result);
    }

    // Test AutoSuggest for the prefix "cat" not present in the 
    // trie containing "catastrophe", "catatonic", and "caterpillar"
    [TestMethod]
    public void AutoSuggest_NotFound()
    {
        Trie dictionary = new Trie();
        dictionary.Insert("catastrophe");
        dictionary.Insert("catatonic");
        dictionary.Insert("caterpillar");
        List<string> suggestions = dictionary.AutoSuggest("cat");
        Assert.AreEqual(3, suggestions.Count);
        Assert.AreEqual("catastrophe", suggestions[0]);
        Assert.AreEqual("catatonic", suggestions[1]);
        Assert.AreEqual("caterpillar", suggestions[2]);
    }

}