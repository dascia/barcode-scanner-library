using System;
using System.IO.Ports;

namespace Dascia.BarcodeScannerLibrary
{
  public class BarcodeScanner : IScanner
  {
    /// <summary>
    /// Indicates if the instance have been disposed already.
    /// </summary>
    bool _disposed;

    /// <summary>
    /// The serial port object used too handle the data received from barcode scanner.
    /// </summary>
    private readonly SerialPort _serialPort;

    /// <summary>
    /// Occurs when the implementation recognizes a barcode from input read.
    /// </summary>
    public event EventHandler<string> BarcodeRecognized;

    /// <summary>
    /// Raises the barcode recognized event.
    /// </summary>
    /// <param name="codeRecognized">The code recognized.</param>
    private void RaiseBarcodeRecognized(string codeRecognized) => BarcodeRecognized?.Invoke(this, codeRecognized);

    /// <summary>
    /// Initializes a new instance of the <see cref="BarcodeScanner" /> class.
    /// </summary>
    /// <param name="port">The COM port where the barcode scanner is connected.</param>
    /// <param name="baudRate">The baud rate.</param>
    public BarcodeScanner(string port, int baudRate = 9600, string terminatingCharacters = "\r")
    {
      _serialPort = new SerialPort(port, baudRate)
      {
        NewLine = terminatingCharacters
      };
      _serialPort.DataReceived += SerialDataReceived;
    }

    /// <summary>
    /// Opens this instance.
    /// </summary>
    public void Initialize()
    {
      if (!_serialPort.IsOpen) _serialPort.Open();
    }

    /// <summary>
    ///  Data received from the scanner through serial port.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="SerialDataReceivedEventArgs"/> instance containing the event data.</param>
    private void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      // EOF = ctrl+Z
      if (e.EventType == SerialData.Eof) return;
      string barcode = _serialPort.ReadLine();
      RaiseBarcodeRecognized(barcode);
    }

    /// <summary>
    /// Dispose the barcode scanner instance.
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (_disposed) return;
      if (disposing)
      {
        _serialPort.DataReceived -= SerialDataReceived;
        if (_serialPort.IsOpen) _serialPort.Close();
      }
      _disposed = true;
    }
  }
}
