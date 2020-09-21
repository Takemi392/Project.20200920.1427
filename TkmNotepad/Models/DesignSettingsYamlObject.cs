using System;

namespace TkmNotepad.Models
{
  public class DesignSettingsYamlObject
  {
    [YamlDotNet.Serialization.YamlMember(Alias = "Background")]
    public string Background { get; set; } = "#000000";

    [YamlDotNet.Serialization.YamlMember(Alias = "FontColor")]
    public string FontColor { get; set; } = "#00AA00";

    [YamlDotNet.Serialization.YamlMember(Alias = "FontSize")]
    public int FontSize { get; set; } = 12;
  }
}
