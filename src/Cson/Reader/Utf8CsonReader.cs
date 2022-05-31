using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Cson.Syntax;

namespace Cson;

public ref struct Utf8CsonReader
{
    private readonly ReadOnlySpan<byte> _buffer;
    private readonly Utf8CsonReaderOptions _options;

    private int _position;

    public Utf8CsonReader(ReadOnlySpan<byte> buffer, Utf8CsonReaderOptions? options = default)
    {
        _buffer = buffer;
        _options = options ?? new Utf8CsonReaderOptions();
        _position = 0;
    }

    public bool IsEmpty => _position >= _buffer.Length;

    public int Position => _position;

    public bool TryRead<TSyntaxNode>([NotNullWhen(true)] out TSyntaxNode? syntax)
        where TSyntaxNode : notnull, CsonSyntaxNode
    {
        var success = TryRead(typeof(TSyntaxNode), out var csonSyntax);
        syntax = (TSyntaxNode?)csonSyntax;
        return success;
    }

    public bool TryRead(Type syntaxType, [NotNullWhen(true)] out CsonSyntaxNode? syntax)
    {
        if (IsEmpty)
        {
            syntax = null;
            return false;
        }

        var syntaxReader = _options.GetReaderFromSyntax(syntaxType);
        if (syntaxReader is null)
        {
            throw new Exception($"Reader for syntax {syntaxType} not found");
        }

        // Skip whitespace characters
        Peek(skipWhitespace: true);

        var readerCopy = this;

        if (!syntaxReader.TryRead(ref this, out syntax))
        {
            this = readerCopy;
            return false;
        }

        return true;
    }

    public bool TryRead([NotNullWhen(true)] out CsonSyntaxNode? syntax)
    {
        if (IsEmpty)
        {
            syntax = null;
            return false;
        }

        // Skip whitespace characters
        Peek(skipWhitespace: true);

        var readerCopy = this;
        var syntaxReaders = _options.GetSyntaxReaders();

        foreach (var syntaxReader in syntaxReaders)
        {
            if (syntaxReader.TryRead(ref this, out syntax))
            {
                return true;
            }

            this = readerCopy;
        }

        syntax = null;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte Consume(int offset = 0, bool skipWhitespace = false)
    {
        var symbol = Peek(offset, skipWhitespace);
        Advance();
        return symbol;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte Peek(int offset = 0, bool skipWhitespace = false)
    {
        if (_position + offset < _buffer.Length)
        {
            var symbol = _buffer[_position + offset];

            if (skipWhitespace)
            {
                for (; !IsEmpty; Advance())
                {
                    symbol = _buffer[_position + offset];

                    if (!CsonHelpers.IsWhitespace(symbol))
                    {
                        break;
                    }
                }
            }

            return symbol;
        }

        return 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<byte> Slice(int start, int length)
    {
        return _buffer.Slice(start, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Advance(int count = 1)
    {
        for (var i = 0; i < count; i++)
        {
            // TODO: Increment line number and column
            if (Peek() == CsonConstants.LineFeed) {}
            else {}

            _position++;
        }
    }
}
