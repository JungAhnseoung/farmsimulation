public interface Save
{
    void Register();
    void Unregister();
    void Store(string scene);
    void Recover(string scene);
    void Load(SaveGame saveGame);
    SaveObject Save();
    string SaveID { get; set; }
    SaveObject SaveObject { get; set; } //to SaveGame : saveObjectDict : SaveObject part ( + guid)

}