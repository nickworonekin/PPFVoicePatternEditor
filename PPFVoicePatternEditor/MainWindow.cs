using System;
using System.Drawing;
using System.Windows.Forms;

namespace PPFVoicePatternEditor
{
    public class MainWindow : Form
    {
        private ComboBox[,] voicePatternBox = new ComboBox[22, 20];

        // Game
        GameFile gameFile;

        // Character Names
        private string[] charNames; // This will point to either CharNamesPPF1 or CharNamesPPF2
        private readonly string[] CharNamesPPF1 = new string[] {
            "Amitie",
            "Oshare Bones",
            "Klug",
            "Dongurigaeru",
            "Rider",
            "Onion Pixy",
            "Ocean Prince",
            "Raffine",
            "Yu",
            "Tarutaru",
            "Hohow Bird",
            "Ms. Accord",
            "Frankensteins",
            "Arle",
            "Popoi",
            "Carbuncle"
        };
        private readonly string[] CharNamesPPF2 = new string[] {
            "Amitie",
            "Oshare Bones",
            "Klug",
            "Dongurigaeru",
            "Rider",
            "Onion Pixy",
            "Ocean Prince",
            "Raffine",
            "Yu",
            "Tarutaru",
            "Hohow Bird",
            "Ms. Accord",
            "Frankensteins",
            "Arle",
            "Sig",
            "Lemres",
            "Feli",
            "Baldanders",
            "Gogotte",
            "Akuma",
            "Strange Klug",
        };
        private readonly string[] voicePatternNum = new string[] {
            "00", "01", "02", "03", "04", "05", 
            "06", "07", "08", "09", "10", "11",
            "Spell #1", "Spell #2", "Spell #3",
            "Spell #4", "Spell #5",
            "12 (Select Character)", "13 (Enter Fever)",
            "14 (Small Nuisance)",   "15 (Large Nuisance)",
            "16 (Win)", "17 (Lose)", "18 (Successful Fever)",
            "19 (Unsuccessful Fever)", "20 (Title)"
        };

        private readonly string[] spellVoiceNum = new string[]{
            "06", "07", "08", "09", "10", "11"
        };

        private readonly string[] animationNum = new string[]{
            "None",
            "Animation #1", "Animation #2", "Animation #3", "Animation #4", "Animation #5",
            "Small Nuisance", "Large Nuisance",
            "Enter Fever"
        };

        public MainWindow()
        {
            this.ClientSize = new Size(640, 458);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = PPFVoicePatternEditor.ProgramName + " v" + PPFVoicePatternEditor.ProgramVersion;
            this.Icon = Resources.ProgramIcon;
            this.Show();
            this.Enabled = false;

            // Before we do anything else, we need to load the game file
            gameFile = new GameFile();

            // Set charNames to the correct array
            if (gameFile.selectedGame == GameFile.Game.PPF1)
                charNames = CharNamesPPF1;
            else if (gameFile.selectedGame == GameFile.Game.PPF2)
                charNames = CharNamesPPF2;

            // Create the Character Selection Box
            ComboBox charSelectBox = new ComboBox();
            charSelectBox.DropDownStyle = ComboBoxStyle.DropDownList;
            charSelectBox.Location = new Point(10, 10);
            charSelectBox.Size = new Size(200, 21);
            charSelectBox.Items.AddRange(charNames);
            charSelectBox.MaxDropDownItems = charNames.Length;
            charSelectBox.SelectedIndex = 0;
            charSelectBox.SelectedIndexChanged += delegate(object sender, EventArgs e)
            {
                GetVoiceData(charSelectBox.SelectedIndex);
            };

            Button aboutButton = new Button();
            aboutButton.UseVisualStyleBackColor = true;
            aboutButton.Text = "About";
            aboutButton.Location = new Point(this.ClientSize.Width - 74, 8);
            aboutButton.Size = new Size(64, 24);
            aboutButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            aboutButton.Click += delegate(object sender, EventArgs e)
            {
                new About();
            };

            Panel charSelectPanel = new Panel();
            charSelectPanel.BackColor = SystemColors.Window;
            charSelectPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            charSelectPanel.Location = new Point(0, 0);
            charSelectPanel.Size = new Size(this.ClientSize.Width, 40);
            charSelectPanel.Controls.Add(charSelectBox);
            charSelectPanel.Controls.Add(aboutButton);
            this.Controls.Add(charSelectPanel);

            // Create the tab control and the tabs
            TabControl tabControl = new TabControl();
            tabControl.Multiline = true;
            tabControl.SizeMode = TabSizeMode.FillToRight;
            tabControl.Location = new Point(8, 48);
            tabControl.Size = new Size(624, 370);
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

            // Create & add the tabs
            string[] tabsText = new string[] {
                "1 Chain", "2 Chain", "3 Chain", "4 Chain", "5 Chain", "6 Chain", "7 Chain", "8 Chain", "9 Chain", "10 Chain",
                "11 Chain", "12 Chain", "13 Chain", "14 Chain", "15 Chain", "16 Chain", "17 Chain", "18 Chain", "19 Chain", "20 Chain",
                "Spells", "Animations",
            };
            for (int i = 0; i < tabsText.Length; i++)
            {
                TabPage tab = new TabPage(tabsText[i]);
                tabControl.TabPages.Add(tab);
                tab.UseVisualStyleBackColor = true;
            }

            // Add content to the first 20 tab pages
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    Label label = new Label();
                    label.Text = (j < 9 ? "  " : "") + "Chain #" + (j + 1) + ":";
                    label.Location = new Point((j > 9 ? 300 : 20), 13 + ((j % 10) * 30));
                    label.Size = new Size(label.Width - 40, label.Height);
                    tabControl.TabPages[i].Controls.Add(label);

                    voicePatternBox[i, j] = new ComboBox();
                    voicePatternBox[i, j].DropDownStyle = ComboBoxStyle.DropDownList;
                    voicePatternBox[i, j].Location = new Point((j > 9 ? 360 : 80), 10 + ((j % 10) * 30));
                    voicePatternBox[i, j].Size = new Size(144, Size.Height);

                    voicePatternBox[i, j].Items.AddRange(voicePatternNum);

                    voicePatternBox[i, j].MaxDropDownItems = voicePatternNum.Length;
                    tabControl.TabPages[i].Controls.Add(voicePatternBox[i, j]);
                }
            }

            // Add content to the "Spells" tab
            for (int i = 0; i < 5; i++)
            {
                Label label = new Label();
                label.Text = "Spell #" + (i + 1) + ":";
                label.Location = new Point(20, 25 + (i * 60));
                tabControl.TabPages[20].Controls.Add(label);

                for (int j = 0; j < 4; j++)
                {
                    // Create yet another label
                    label = new Label();
                    label.Location = new Point(64 + (j < 2 ? 20 : 292), 13 + (i * 60) + (j % 2 == 0 ? 0 : 22));
                    label.Size = new Size(120, label.Height);
                    label.TextAlign = ContentAlignment.TopRight;

                    switch (j)
                    {
                        case 0: label.Text = "4 Puyo:"; break;
                        case 1: label.Text = "5 or 6 Puyo:"; break;
                        case 2: label.Text = "7 to 10 Puyo:"; break;
                        case 3: label.Text = "11 or more Puyo:"; break;
                    }

                    tabControl.TabPages[20].Controls.Add(label);

                    voicePatternBox[20, (i * 4) + j] = new ComboBox();
                    voicePatternBox[20, (i * 4) + j].DropDownStyle = ComboBoxStyle.DropDownList;
                    voicePatternBox[20, (i * 4) + j].Items.AddRange(spellVoiceNum);
                    voicePatternBox[20, (i * 4) + j].MaxDropDownItems = spellVoiceNum.Length;
                    voicePatternBox[20, (i * 4) + j].Location = new Point(130 + (j < 2 ? 80 : 352), 10 + (i * 60) + (j % 2 == 0 ? 0 : 22));
                    tabControl.TabPages[20].Controls.Add(voicePatternBox[20, (i * 4) + j]);
                }
            }

            // Add content to the "Animations" tab
            for (int i = 0; i < 6; i++)
            {
                Label label = new Label();
                label.Text = (i + 6 < 10 ? "  " : "") + "Voice #" + (i + 6) + ":";
                label.Location = new Point(20, 13 + (i * 30));
                label.Size = new Size(60, label.Height);
                tabControl.TabPages[21].Controls.Add(label);

                voicePatternBox[21, i] = new ComboBox();
                voicePatternBox[21, i].DropDownStyle = ComboBoxStyle.DropDownList;
                voicePatternBox[21, i].Items.AddRange(animationNum);
                voicePatternBox[21, i].MaxDropDownItems = animationNum.Length;
                voicePatternBox[21, i].Location = new Point(80, 10 + (i * 30));
                voicePatternBox[21, i].Size = new Size(160, voicePatternBox[21, i].Height);
                tabControl.TabPages[21].Controls.Add(voicePatternBox[21, i]);
            }

            this.Controls.Add(tabControl);

            // Bottom Panel
            Panel bottomPanel = new Panel();
            bottomPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            bottomPanel.Location = new Point((this.ClientSize.Width / 2) - 104, 426);
            bottomPanel.Size = new Size(208, 24);
            bottomPanel.SizeChanged += delegate(object sender, EventArgs e)
            {
                bottomPanel.Left = (this.ClientSize.Width / 2) - 104;
            };
            this.Controls.Add(bottomPanel);

            // Let's add the buttons at the bottom.
            Button importButton = new Button();
            importButton.Text = "Import";
            importButton.Location = new Point(0, 0);
            importButton.Size = new Size(64, 24);
            bottomPanel.Controls.Add(importButton);
            importButton.Click += ImportData;

            Button saveButton = new Button();
            saveButton.Text = "Save";
            saveButton.Location = new Point(72, 0);
            saveButton.Size = new Size(64, 24);
            saveButton.Click += delegate(object sender, EventArgs e)
            {
                SetVoiceData(charSelectBox.SelectedIndex);
                gameFile.Save();
                MessageBox.Show("Voice pattern data saved successfully.", "Saved");
            };
            bottomPanel.Controls.Add(saveButton);

            Button exportButton = new Button();
            exportButton.Text = "Export";
            exportButton.Location = new Point(144, 0);
            exportButton.Size = new Size(64, 24);
            bottomPanel.Controls.Add(exportButton);
            exportButton.Click += ExportData;

            GetVoiceData(charSelectBox.SelectedIndex);

            this.Enabled = true;
        }

        // Get the voice data for the selected chararacter
        private void GetVoiceData(int character)
        {
            // Voice Pattern Data
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    // Set up the correct voice patterns
                    voicePatternBox[i, j].SelectedIndex = VoicePatternDataToIndex(gameFile.Data[gameFile.OffsetStart + (character * 0x640) + (i * gameFile.VoiceIncAmount) + (j * 4)]);
                }
            }

            // Spells
            for (int i = 0; i < 20; i++)
                voicePatternBox[20, i].SelectedIndex = SpellDataToIndex(gameFile.Data[gameFile.OffsetStart + gameFile.OffsetEnderVoices + (character * 0x50) + (i * 4)]);

            // Animations
            for (int i = 0; i < 6; i++)
                voicePatternBox[21, i].SelectedIndex = AnimationDataToIndex(BitConverter.ToInt32(gameFile.Data, gameFile.OffsetStart + gameFile.OffsetEnderImages + (character * 0x18) + (i * 4)));
        }

        // Set the voice data for the selected character
        private void SetVoiceData(int character)
        {
            // Voice Pattern Data
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < i + 1; j++)
                    gameFile.Data[gameFile.OffsetStart + (character * 0x640) + (i * gameFile.VoiceIncAmount) + (j * 4)] = VoicePatternIndexToData(voicePatternBox[i, j].SelectedIndex);
            }

            // Spells
            for (int i = 0; i < 20; i++)
                gameFile.Data[gameFile.OffsetStart + gameFile.OffsetEnderVoices + (character * 0x50) + (i * 4)] = SpellIndexToData(voicePatternBox[20, i].SelectedIndex);

            // Animations
            for (int i = 0; i < 6; i++)
                BitConverter.GetBytes(AnimationIndexToData(voicePatternBox[21, i].SelectedIndex)).CopyTo(gameFile.Data, gameFile.OffsetStart + gameFile.OffsetEnderImages + (character * 0x18) + (i * 4));
        }

        // Get voice pattern data from an index
        private byte VoicePatternIndexToData(int value)
        {
            switch (value)
            {
                case 1: return 1;
                case 2: return 2;
                case 3: return 3;
                case 4: return 4;
                case 5: return 5;
                case 6: return 6;
                case 7: return 7;
                case 8: return 8;
                case 9: return 9;
                case 10: return 10;
                case 11: return 11;
                case 12: return 0x80;
                case 13: return 0x81;
                case 14: return 0x82;
                case 15: return 0x83;
                case 16: return 0x84;
                case 17: return 12;
                case 18: return 13;
                case 19: return 14;
                case 20: return 15;
                case 21: return 16;
                case 22: return 17;
                case 23: return 18;
                case 24: return 19;
                case 25: return 20;
                default: return 0;
            }
        }

        // Get voice pattern index from data
        private int VoicePatternDataToIndex(byte value)
        {
            switch (value)
            {
                case 1: return 1;
                case 2: return 2;
                case 3: return 3;
                case 4: return 4;
                case 5: return 5;
                case 6: return 6;
                case 7: return 7;
                case 8: return 8;
                case 9: return 9;
                case 10: return 10;
                case 11: return 11;
                case 0x80: return 12;
                case 0x81: return 13;
                case 0x82: return 14;
                case 0x83: return 15;
                case 0x84: return 16;
                case 12: return 17;
                case 13: return 18;
                case 14: return 19;
                case 15: return 20;
                case 16: return 21;
                case 17: return 22;
                case 18: return 23;
                case 19: return 24;
                case 20: return 25;
                default: return 0;
            }
        }

        // Get spell data from an index
        private byte SpellIndexToData(int value)
        {
            switch (value)
            {
                case 1: return 7;
                case 2: return 8;
                case 3: return 9;
                case 4: return 10;
                case 5: return 11;
                default: return 6;
            }
        }

        // Get spell index from data
        private int SpellDataToIndex(byte value)
        {
            switch (value)
            {
                case 7: return 1;
                case 8: return 2;
                case 9: return 3;
                case 10: return 4;
                case 11: return 5;
                default: return 0;
            }
        }

        // Get animation box value
        private int AnimationIndexToData(int value)
        {
            switch (value)
            {
                case 1: return 0;
                case 2: return 1;
                case 3: return 2;
                case 4: return 3;
                case 5: return 4;
                case 6: return 5;
                case 7: return 6;
                case 8: return 9;
                default: return -1; // Same as 0xFFFFFFFF
            }
        }

        // Set animation box value
        private int AnimationDataToIndex(int value)
        {
            switch (value)
            {
                case 0: return 1;
                case 1: return 2;
                case 2: return 3;
                case 3: return 4;
                case 4: return 5;
                case 5: return 6;
                case 6: return 7;
                case 9: return 8;
                default: return 0;
            }
        }

        // Import Data
        private void ImportData(object sender, EventArgs e)
        {
            byte[] data;
            bool success = ImportExport.Import(out data);

            if (!success)
                return;

            // Read in Voice Patterns
            int pos = 6;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (j >= (i + 1))
                        continue;
                    else
                    {
                        voicePatternBox[i, j].SelectedIndex = VoicePatternDataToIndex(data[pos]);
                        pos++;
                    }
                }
            }

            // Read in spell data and animation data
            for (int i = 0; i < 20; i++)
            {
                voicePatternBox[20, i].SelectedIndex = SpellDataToIndex(data[pos]);
                pos++;
            }

            for (int i = 0; i < 6; i++)
            {
                voicePatternBox[21, i].SelectedIndex = AnimationDataToIndex(data[pos]);
                pos++;
            }
        }

        // Export Data
        private void ExportData(object sender, EventArgs e)
        {
            byte[] data = new byte[236]; // We only make v2 file now, so this is OK
            int pos = 0;

            // Write the Voice Patterns
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    data[pos] = VoicePatternIndexToData(voicePatternBox[i, j].SelectedIndex);
                    pos++;
                }
            }

            // Write spell data
            for (int i = 0; i < 20; i++)
            {
                data[pos] = SpellIndexToData(voicePatternBox[20, i].SelectedIndex);
                pos++;
            }

            // Write animation data
            for (int i = 0; i < 6; i++)
            {
                data[pos] = BitConverter.GetBytes(AnimationIndexToData(voicePatternBox[21, i].SelectedIndex))[0];
                pos++;
            }

            // Now we can export it
            ImportExport.Export(data);
        }
    }
}