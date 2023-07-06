using System;

namespace Dascia.BarcodeScannerLibrary
{
  /// <summary>
  /// Interface for all the barcode scanner implementations.
  /// </summary>
  public interface IScanner : IDisposable
  {
    /// <summary>
    /// Occurs when the implementation recognizes a barcode from input read.
    /// </summary>
    event EventHandler<string> BarcodeRecognized;

    /// <summary>
    /// Opens the port connected to the barcode scanner to start listening for data.
    /// </summary>
    void Initialize(string port, int baudRate = 9600, string terminatingCharacters = "\r");
  }
}
