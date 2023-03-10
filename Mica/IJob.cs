namespace Mica
{
    public interface IJob
    {
        Task Work(IProgress<double> progress, CancellationToken token = default);
    }
}
