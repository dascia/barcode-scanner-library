using Dascia.BarcodeScannerLibrary;

namespace BarcodeScannerTest
{
  public class ScannerTest
  {

    /// <summary>
    /// Test the barcode recognition.
    /// </summary>
    [Theory]
    [InlineData("COM3", 9600, "\r")]
    public void BarcodeRecognizedTest(string port, int baudRate, string terminatingCharacter)
    {
      BarcodeScanner scanner = new BarcodeScanner(port, baudRate, terminatingCharacter);
      AutoResetEvent resetEvent = new AutoResetEvent(false);
      string? barCode = null;
      scanner.BarcodeRecognized += (x, y) =>
      {
        barCode = y;
        resetEvent.Set();
      };
      scanner.Initialize();
      resetEvent.WaitOne(TimeSpan.FromSeconds(60));
      scanner.Dispose();
      Assert.NotNull(barCode);
    }
  }
}