namespace CovrMe.ApplicationHelpers;
public static class AssetsHelper
{
    public static async Task<string> LoadAssets(string path)
    {
        string result = string.Empty;
        try
        {
            using var stream = await FileSystem.Current.OpenAppPackageFileAsync(path);
            using var reader = new StreamReader(stream);

            var contents = reader.ReadToEnd();

            result = contents;

            return result;
        }
        catch (Exception ex)
        {
            return result;
        }
    }
}
