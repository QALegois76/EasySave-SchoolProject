namespace LibEasySave
{
    public interface ICyptPlugin
    {
        void Crypt(string srcFile, string destFile, string key);
        void DeCrypt(string srcFile, string destFile, string key);
    }
}
