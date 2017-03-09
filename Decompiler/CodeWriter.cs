using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teh.Decompiler {
    public class CodeWriter {

        public StreamWriter BaseWriter { get; }
        public int IndentLevel { get; set; } = 0;
        public string Tab { get; set; } = "    ";

        public CodeWriter(StreamWriter writer) {
            this.BaseWriter = writer;
        }

        public void WriteIndent() {
            for (int i = 0; i < IndentLevel; i++) BaseWriter.Write(Tab);
        }

        public void AddIndent() {
            IndentLevel++;
        }

        public void RemoveIndent() {
            IndentLevel--;
        }

        public void Write(string value) {
            BaseWriter.Write(value);
        }

        public void Write(string value, params object[] arg) {
            BaseWriter.Write(value, arg);
        }

        public void WriteLine(string value) {
            WriteIndent();
            BaseWriter.WriteLine(value);
        }

        public void WriteLine(string value, params object[] arg) {
            WriteIndent();
            BaseWriter.WriteLine(value, arg);
        }

        public void WriteLineWithoutIndent(string value) {
            BaseWriter.WriteLine(value);
        }

        public void WriteLineWithoutIndent(string value, params object[] arg) {
            BaseWriter.WriteLine(value, arg);
        }

        public void WriteLine() {
            BaseWriter.WriteLine();
        }
    }
}
