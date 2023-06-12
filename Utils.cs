using System.Text.Json;

namespace TextBasedAdventureGame
{
    public static class Utils
    {
        //Deserialisation method for any data type
        public static T LoadObject<T>(string filePath)
        {
            return JsonSerializer.Deserialize<T>(File.ReadAllText(filePath))!;
        }

        public static List<T> LoadAllObjectsIntoList<T>(string folderPath)
        {
            List<T> list = new List<T>();
            foreach (string s in Directory.GetFiles(folderPath))
            {
                if (s.EndsWith(".json"))
                {
                    T listItem = LoadObject<T>(s);
                    list.Add(listItem);
                }
            }
            return list;
        }
    }
}
