namespace Patterns_Example.Creational_Design_Patterns.Prototype;

/// <summary>
/// Concrete implementation of a presentation document.
/// Demonstrates prototype pattern with slide-based content.
/// </summary>
public class PresentationDocument : Document
{
    public override string DocumentType => "Presentation";
    public List<Slide> Slides { get; set; } = new();
    public string Theme { get; set; } = "Default";
    public string TransitionEffect { get; set; } = "None";
    public bool HasAnimations { get; set; }
    public bool HasSpeakerNotes { get; set; }
    public int CurrentSlide { get; set; } = 1;

    public PresentationDocument() : base() 
    {
        // Add a default slide
        Slides.Add(new Slide { Title = "Title Slide", Content = "Click to add content" });
    }

    public PresentationDocument(string title) : base(title, "Presentation Content") 
    {
        // Add a default slide
        Slides.Add(new Slide { Title = title, Content = "Click to add content" });
    }

    public override Document DeepClone()
    {
        var clone = new PresentationDocument
        {
            Id = Guid.NewGuid().ToString(),
            Title = Title,
            Content = Content,
            Metadata = Metadata.DeepClone(),
            Attachments = new List<string>(Attachments),
            Slides = Slides.Select(s => s.DeepClone()).ToList(),
            Theme = Theme,
            TransitionEffect = TransitionEffect,
            HasAnimations = HasAnimations,
            HasSpeakerNotes = HasSpeakerNotes,
            CurrentSlide = CurrentSlide
        };
        
        return clone;
    }

    public void AddSlide(string title, string content)
    {
        Slides.Add(new Slide { Title = title, Content = content });
        UpdateLastModified();
    }

    public void RemoveSlide(int index)
    {
        if (index >= 0 && index < Slides.Count)
        {
            Slides.RemoveAt(index);
            if (CurrentSlide > Slides.Count)
                CurrentSlide = Math.Max(1, Slides.Count);
            UpdateLastModified();
        }
    }

    public void SetTheme(string theme)
    {
        Theme = theme;
        UpdateLastModified();
    }

    public void SetTransitionEffect(string effect)
    {
        TransitionEffect = effect;
        UpdateLastModified();
    }

    public override string ToString()
    {
        var baseStr = base.ToString();
        var slideCount = Slides.Count;
        var slideTitles = Slides.Take(3).Select(s => s.Title);
        var slidesPreview = slideCount > 3 
            ? string.Join(", ", slideTitles) + $", ... ({slideCount - 3} more)"
            : string.Join(", ", slideTitles);
        
        var presentationInfo = $"""
            Presentation Info:
              Slides: {slideCount}
              Current Slide: {CurrentSlide}
              Theme: {Theme}
              Transition: {TransitionEffect}
              Has Animations: {HasAnimations}
              Has Speaker Notes: {HasSpeakerNotes}
              Slide Titles: {slidesPreview}
            """;
        
        return baseStr + presentationInfo;
    }
}

/// <summary>
/// Represents a single slide in a presentation.
/// </summary>
public class Slide : IPrototype<Slide>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> BulletPoints { get; set; } = new();
    public string BackgroundColor { get; set; } = "White";
    public bool HasImage { get; set; }
    public string ImagePath { get; set; } = string.Empty;

    public Slide Clone()
    {
        return (Slide)MemberwiseClone();
    }

    public Slide DeepClone()
    {
        return new Slide
        {
            Title = Title,
            Content = Content,
            BulletPoints = new List<string>(BulletPoints),
            BackgroundColor = BackgroundColor,
            HasImage = HasImage,
            ImagePath = ImagePath
        };
    }

    public override string ToString()
    {
        var bulletStr = BulletPoints.Count > 0 ? string.Join(", ", BulletPoints) : "None";
        return $"Slide: {Title} | Content: {Content} | Bullets: {bulletStr}";
    }
}
