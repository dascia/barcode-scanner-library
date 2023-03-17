using System;
using System.IO.Ports;

namespace Dascia.BarcodeScannerLibrary
{
  public class BarcodeScanner : IScanner
  {
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
    public void Open()
    {
      if (!_serialPort.IsOpen) _serialPort.Open();
    }

    /// <summary>
    ///  Data received from the scanner through serial port.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="SerialDataReceivedEventArgs"/> instance containing the event data.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    private void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      // EOF = ctrl+Z
      if (e.EventType == SerialData.Eof) return;
      string barcode = _serialPort.ReadLine();
      RaiseBarcodeRecognized(barcode);
    }

    /// <summary>
    /// Closess the port connected to the barcode scanner to stop listening for data.
    /// </summary>
    public void Close()
    {
      if (_serialPort.IsOpen) _serialPort.Close();
    }
  }
}
