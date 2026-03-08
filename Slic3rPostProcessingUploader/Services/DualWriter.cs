using System.Text;

namespace Slic3rPostProcessingUploader.Services;

internal class DualWriter : TextWriter
{
    private readonly TextWriter _console;
    private readonly TextWriter _file;

    public DualWriter(TextWriter console, TextWriter file)
    {
        _console = console;
        _file = file;
    }

    public override Encoding Encoding => _console.Encoding;

    public override void Write(char value)
    {
        _console.Write(value);
        _file.Write(value);
    }

    public override void Write(string? value)
    {
        _console.Write(value);
        _file.Write(value);
    }

    public override void WriteLine(string? value)
    {
        _console.WriteLine(value);
        _file.WriteLine(value);
    }

    public override void Flush()
    {
        _console.Flush();
        _file.Flush();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _file.Dispose();
        }
        base.Dispose(disposing);
    }
}
