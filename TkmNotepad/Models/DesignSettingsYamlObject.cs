using System;

namespace TkmNotepad.Models
{
  public class DesignSettingsYamlObject
  {
    [YamlDotNet.Serialization.YamlMember(Alias = "TextAreaDesign")]
    public TextAreaDesignSettingsYamlObject TextAreaDesignSettings { get; set; } = new TextAreaDesignSettingsYamlObject();

    public class TextAreaDesignSettingsYamlObject
    {
      [YamlDotNet.Serialization.YamlMember(Alias = "Background")]
      public string Background { get; set; } = "#000000";

      [YamlDotNet.Serialization.YamlMember(Alias = "FontColor")]
      public string FontColor { get; set; } = "#00AA00";

      [YamlDotNet.Serialization.YamlMember(Alias = "FontSize")]
      public double FontSize { get; set; } = 16.0;

      [YamlDotNet.Serialization.YamlMember(Alias = "FontFamily")]
      public string FontFamily { get; set; } = "Consolas";
    }
  }
}
