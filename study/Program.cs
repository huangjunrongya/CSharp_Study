class Program
{
    static async Task Main(string[] args)
    {
        await UseTheAnswerAsync();
    }

    public static async Task<int> GetTheAnswerAsync()
    {
        await Task.Delay(5000); // 异步等待5秒
        return 42; // 返回结果
    }

    public static async Task UseTheAnswerAsync()
    {
        int answer = await GetTheAnswerAsync();
        Console.WriteLine($"The answer is {answer}");
    }
}