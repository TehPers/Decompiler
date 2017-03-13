using Mono.Cecil;
using Mono.Cecil.Cil;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Teh.Decompiler;
using Teh.Decompiler.Builders;
using Teh.Decompiler.Graphs;

namespace DecompilerInterface {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();

            tvAssemblyViewer.NodeMouseDoubleClick += TvAssemblyViewer_NodeMouseDoubleClick;
        }

        private void MainForm_Load(object sender, EventArgs e) {

        }

        private void OpenAssemblyToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.ofdAssembly.ShowDialog() == DialogResult.OK) {
                AddAssembly(AssemblyDefinition.ReadAssembly(this.ofdAssembly.FileName));
            }
        }

        private void TvAssemblyViewer_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e) {
            if (e.Node is DefinitionTreeNode) {
                DefinitionTreeNode node = e.Node as DefinitionTreeNode;
                if (node.Definition is TypeDefinition) {
                    TypeDefinition type = node.Definition as TypeDefinition;
                    // Get code
                    StringBuilder code = new StringBuilder();
                    using (StringWriter stream = new StringWriter(code)) {
                        CodeWriter writer = new CodeWriter(stream);
                        TypeBuilder builder = new TypeBuilder(type);
                        builder.Build(writer);
                    }

                    // Create Scintilla code viewer
                    Scintilla scintilla = new Scintilla() {
                        Dock = DockStyle.Fill,
                        Text = code.ToString()
                    };

                    const int padding = 2;
                    scintilla.Margins[0].Width = scintilla.TextWidth(Style.LineNumber, new string('9', scintilla.Lines.Count.ToString().Length + 1)) + padding;
                    scintilla.Margins[1].Width = 0;
                    scintilla.Margins[2].Width = 0;
                    scintilla.Margins[3].Width = 0;

                    SetScintillaStyles(scintilla);
                    scintilla.ReadOnly = true;

                    TabPage tab = new TabPage(type.Name);
                    tab.Controls.Add(scintilla);
                    tcOutput.TabPages.Add(tab);
                } else if (node.Definition is MethodDefinition method) {
                    ILGraph graph = new ILGraph(method.Body.Instructions);

                    TabPage tab = new TabPage(method.Name);
                    tab.Controls.Add(new ILGraphViewer(graph) {
                        Location = Point.Empty,
                        Anchor = AnchorStyles.Top | AnchorStyles.Left
                    });
                    tab.AutoScroll = true;
                    tcOutput.TabPages.Add(tab);
                }
            }
        }

        private void SetScintillaStyles(Scintilla scintilla) {
            scintilla.StyleResetDefault();
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 10;
            scintilla.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            scintilla.Lexer = Lexer.Cpp;
            scintilla.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            scintilla.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
            scintilla.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
            scintilla.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            scintilla.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
            scintilla.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
            scintilla.Styles[Style.Cpp.Operator].ForeColor = Color.Purple;
            scintilla.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;

            // Set the keywords
            scintilla.SetKeywords(0, "abstract as base break case catch checked continue default delegate do else event explicit extern false finally fixed for foreach goto if implicit in interface internal is lock namespace new null object operator out override params private protected public readonly ref return sealed sizeof stackalloc switch this throw true try typeof unchecked unsafe using virtual while");
            scintilla.SetKeywords(1, "bool byte char class const decimal double enum float int long sbyte short static string struct uint ulong ushort void");

            // Instruct the lexer to calculate folding
            scintilla.SetProperty("fold", "1");
            scintilla.SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            scintilla.Margins[2].Type = MarginType.Symbol;
            scintilla.Margins[2].Mask = Marker.MaskFolders;
            scintilla.Margins[2].Sensitive = true;
            scintilla.Margins[2].Width = 20;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++) {
                scintilla.Markers[i].SetForeColor(SystemColors.ControlLightLight);
                scintilla.Markers[i].SetBackColor(SystemColors.ControlDark);
            }

            // Configure folding markers with respective symbols
            scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            scintilla.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            scintilla.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);
        }

        private void AddAssembly(AssemblyDefinition assembly) {
            TreeNode assemblyNode = new TreeNode(assembly.Name.Name);
            this.tvAssemblyViewer.Nodes.Add(assemblyNode);
            foreach (ModuleDefinition module in assembly.Modules) {
                TreeNode moduleNode = new TreeNode(module.Name);
                assemblyNode.Nodes.Add(moduleNode);

                foreach (String nspace in module.Types.Select(t => t.Namespace).Distinct()) {
                    TreeNode namespaceNode = new TreeNode(nspace);
                    moduleNode.Nodes.Add(namespaceNode);

                    foreach (TypeDefinition type in module.Types.Where(t => t.Namespace == nspace)) {
                        TreeNode typeNode = new DefinitionTreeNode(type);
                        namespaceNode.Nodes.Add(typeNode);

                        foreach (MethodDefinition method in type.Methods) {
                            TreeNode methodNode = new DefinitionTreeNode(method);
                            typeNode.Nodes.Add(methodNode);
                        }
                    }
                }
            }
        }

        private class DefinitionTreeNode : TreeNode {
            public IMemberDefinition Definition { get; set; }

            public DefinitionTreeNode(IMemberDefinition definition) {
                this.Definition = definition;
                this.Text = definition.Name;
            }
        }
    }
}
