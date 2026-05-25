using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MonoGame.Template.TwoD.UI;

public interface IUIService
{
    IReadOnlyCollection<SpriteFont> GetAllSpriteFonts();
    SpriteFont? GetSpriteFontByName(string name);
    void RegisterSpriteFont(SpriteFont spriteFont, string name);
    bool RemoveSpriteFont(string name);
}

public class UIService : IUIService
{
    private readonly Dictionary<string, SpriteFont> _fonts = new();

    public UIService()
    {
        _fonts = [];
    }

    public IReadOnlyCollection<SpriteFont> GetAllSpriteFonts()
    {
        return new ReadOnlyCollection<SpriteFont>([.. _fonts.Values]);
    }

    public SpriteFont? GetSpriteFontByName(string name)
    {
        return _fonts.TryGetValue(name, out var font) ? font : null;
    }

    public void RegisterSpriteFont(SpriteFont spriteFont, string name)
    {
        ArgumentNullException.ThrowIfNull(spriteFont);

        if (!_fonts.TryAdd(name, spriteFont))
        {
            throw new InvalidOperationException($"A font with name '{name}' is already registered.");
        }
    }

    public bool RemoveSpriteFont(string name)
    {
        var font = _fonts.TryGetValue(name, out var e) ? e : null;

        if (font is null)
        {
            return false;
        }

        _fonts.Remove(name);
        return true;
    }
}

