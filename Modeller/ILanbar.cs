public interface ILanbar
{
    string Id { get; }
    string Tittel { get; }
    bool LaanUt();
    void LeverInn();
}
