using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.UI;

namespace AK.Toolkit.WinUI3.ButtonExtensions;

public static class ButtonExtensions
{
    public static readonly DependencyProperty PointerOverBackgroundLightnessFactorProperty =
        DependencyProperty.RegisterAttached(
            "PointerOverBackgroundLightnessFactor",
            typeof(double),
            typeof(ButtonExtensions),
            new PropertyMetadata(default, (d, e) =>
            {
                HandleBackgroundLightnessFactorPropertyChanged(d, e, "ButtonBackgroundPointerOver");
            }));

    public static readonly DependencyProperty PressedBackgroundLightnessFactorProperty =
        DependencyProperty.RegisterAttached(
            "PressedBackgroundLightnessFactor",
            typeof(double),
            typeof(ButtonExtensions),
            new PropertyMetadata(default, (d, e) =>
            {
                HandleBackgroundLightnessFactorPropertyChanged(d, e, "ButtonBackgroundPressed");
            }));

    public static readonly DependencyProperty PointerOverForegroundLightnessFactorProperty =
        DependencyProperty.RegisterAttached(
            "PointerOverForegroundLightnessFactor",
            typeof(double),
            typeof(ButtonExtensions),
            new PropertyMetadata(default, (d, e) =>
            {
                HandleForegroundLightnessFactorPropertyChanged(d, e, "ButtonForegroundPointerOver");
            }));

    public static readonly DependencyProperty PressedForegroundLightnessFactorProperty =
        DependencyProperty.RegisterAttached(
            "PressedForegroundLightnessFactor",
            typeof(double),
            typeof(ButtonExtensions),
            new PropertyMetadata(default, (d, e) =>
            {
                HandleForegroundLightnessFactorPropertyChanged(d, e, "ButtonForegroundPressed");
            }));

    public static double GetPointerOverBackgroundLightnessFactor(DependencyObject obj) => (double)obj.GetValue(PointerOverBackgroundLightnessFactorProperty);

    public static void SetPointerOverBackgroundLightnessFactor(DependencyObject obj, double value) => obj.SetValue(PointerOverBackgroundLightnessFactorProperty, value);

    public static double GetPressedBackgroundLightnessFactor(DependencyObject obj) => (double)obj.GetValue(PressedBackgroundLightnessFactorProperty);

    public static void SetPressedBackgroundLightnessFactor(DependencyObject obj, double value) => obj.SetValue(PressedBackgroundLightnessFactorProperty, value);

    public static double GetPointerOverForegroundLightnessFactor(DependencyObject obj) => (double)obj.GetValue(PointerOverForegroundLightnessFactorProperty);

    public static void SetPointerOverForegroundLightnessFactor(DependencyObject obj, double value) => obj.SetValue(PointerOverForegroundLightnessFactorProperty, value);

    public static double GetPressedForegroundLightnessFactor(DependencyObject obj) => (double)obj.GetValue(PressedForegroundLightnessFactorProperty);

    public static void SetPressedForegroundLightnessFactor(DependencyObject obj, double value) => obj.SetValue(PressedForegroundLightnessFactorProperty, value);

    private static void HandleBackgroundLightnessFactorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, string resourceKey)
    {
        if (d is not Button button ||
            e.NewValue is not double brushLightnessFactor)
        {
            return;
        }

        _ = button.RegisterPropertyChangedCallback(
            Button.BackgroundProperty, (_, _) =>
            {
                UpdateResourceBrushLightness(button, Button.BackgroundProperty, resourceKey, brushLightnessFactor);
            });

        UpdateResourceBrushLightness(button, Button.BackgroundProperty, resourceKey, brushLightnessFactor);
    }

    private static void HandleForegroundLightnessFactorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, string resourceKey)
    {
        if (d is not Button button ||
            e.NewValue is not double brushLightnessFactor)
        {
            return;
        }

        _ = button.RegisterPropertyChangedCallback(
            Button.ForegroundProperty, (_, _) =>
            {
                UpdateResourceBrushLightness(button, Button.ForegroundProperty, resourceKey, brushLightnessFactor);
            });

        UpdateResourceBrushLightness(button, Button.ForegroundProperty, resourceKey, brushLightnessFactor);
    }

    private static void UpdateResourceBrushLightness(Button button, DependencyProperty brushDependencyProperty, string resourceKey, double brushLightnessFactor)
    {
        if (button.GetValue(brushDependencyProperty) is not SolidColorBrush brush)
        {
            return;
        }

        HslColor hsl = brush.Color.ToHsl();
        hsl.L = Math.Max(Math.Min(hsl.L * brushLightnessFactor, 1.0), 0.0);
        Color newColor = ColorHelper.FromHsl(
            hue: hsl.H,
            saturation: hsl.S,
            lightness: hsl.L);

        button.Resources[resourceKey] = new SolidColorBrush(newColor);
    }
}
