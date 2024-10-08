using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using ImGuiScene;
using System.Collections.Generic;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using System.Diagnostics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Textures;

namespace LogogramHelper.Windows;

public class MainWindow : Window, IDisposable
{
    private Plugin Plugin { get; }
    private List<LogosAction> LogosActions { get; }

    public MainWindow(Plugin plugin) : base(
        "文理技能", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.AlwaysAutoResize)
    {
        this.Plugin = plugin;
        this.LogosActions = plugin.LogosActions;
        this.ShowCloseButton = false;
    }

    public void Dispose()
    {
    }

    private string filter = "";

    public override unsafe void Draw()
    {
        var fontScaling = ImGui.GetFontSize() / 17;

        ImGui.PushItemWidth(400);
        ImGui.InputTextWithHint("", "搜索文理技能...", ref filter, 50, ImGuiInputTextFlags.AutoSelectAll);
        ImGui.PopItemWidth();

        ImGui.SameLine();

        if (ImGuiComponents.IconButton("KoFi", FontAwesomeIcon.Coffee, new Vector4(1.0f, 0.35f, 0.37f, 1.0f)))
            Process.Start(new ProcessStartInfo { FileName = "https://ko-fi.com/apetih", UseShellExecute = true });
        if (ImGui.IsItemHovered())
            ImGui.SetTooltip("Support me on Ko-Fi");


        for (var i = 0; i < 56; i++)
        {
            var action = LogosActions[i];
            var padding = 2;
            var bg = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            var tint = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            if (!action.Name.ToLower().Contains(filter.ToLower())) tint.W = 0.25f;
            if (ImGui.ImageButton(Plugin.TextureProvider.GetFromGameIcon(action.IconID).GetWrapOrEmpty().ImGuiHandle, new Vector2(40, 40) * fontScaling, new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), padding, bg, tint))
            {
                /*var roleTextures = new Dictionary<uint, ISharedImmediateTexture>();
                action.Roles.ForEach(role =>
                {
                    var tex = Plugin.TextureProvider.GetFromGameIcon(role);
                    roleTextures.Add(role, tex);
                });*/
                Plugin.DrawLogosDetailUI(action);
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip($"{action.Name}");
            if ((i + 1) % 10 != 0) ImGui.SameLine();
        }

    }
}
