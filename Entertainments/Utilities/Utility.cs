using System.Data;

namespace Entertainments.Utilities
{
    public class Utility
    {
        public async Task ClearUpdateStatusAsync(string updateStatus)
        {
            await Task.Delay(5000); // 10 seconds

            updateStatus = string.Empty;

            //await InvokeAsync(StateHasChanged);
        }
    }
}
