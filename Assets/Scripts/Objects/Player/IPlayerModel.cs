public interface IPlayerModel
{
    event System.Action<float, float> OnUpdateHpAction;
    event System.Action<long> OnUpdateGoldAction;

    void Init();
}
