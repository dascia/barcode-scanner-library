using System;

namespace Dascia.BarcodeScannerLibrary
{
  /// <summary>
  /// Interface for all the barcode scanner implementations.
  /// </summary>
  public interface IScanner
  {
    /// <summary>
    /// Occurs when the implementation recognizes a barcode from input read.
    /// </summary>
    event EventHandler<string> BarcodeRecognized;

    /// <summary>
    /// Opens the port connected to the barcode scanner to start listening for data.
    /// </summary>
    void Open();

    /// <summary>
    /// Closess the port connected to the barcode scanner to stop listening for data.
    /// </summary>
    void Close();
  }
}
