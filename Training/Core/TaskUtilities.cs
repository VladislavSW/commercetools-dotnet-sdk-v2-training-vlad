using System;
using System.Threading.Tasks;
using commercetools.Base.Client.Error;

namespace Training
{
    public static class TaskUtilities
    {
        public static async Task FireAndForgetSafeAsync(this Task task)
        {
            try
            {
                await task;
            }
            catch (ApiClientException ex) {
                Console.WriteLine(ex.Body);
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
