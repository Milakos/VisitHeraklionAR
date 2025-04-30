using System;

[Serializable]
public class TextClassTranslate
{
    public string greekText;
    public string englishText;
    
    /// <summary>
    /// Constructor Class taking two string arguments for translation puproses. Flip Flop between two languages
    /// </summary>
    /// <param name="greekText"></param>
    /// <param name="englishText"></param>
    public TextClassTranslate(string greekText, string englishText) 
    {
        this.greekText = greekText;
        this.englishText = englishText;
    }
    /// <summary>
    /// Translates between two languages English and Greek. 
    /// </summary>
    /// <returns></returns>
    public string GetTranslatedText()
    {
        return UIExtentions.IsEnglish() ? englishText : greekText;
    }
    /// <summary>
    /// Translates between two languages English and Greek. With two input Values
    /// </summary>
    /// <param name="englishText"></param>
    /// <param name="greekText"></param>
    /// <returns></returns>
    public string GetTranslatedText(string englishText, string greekText)
    {
        return  UIExtentions.IsEnglish() ? englishText : greekText;
    } 
    /// <summary>
    /// Retrieves a translated text based on the current language setting (English or Greek).
    /// </summary>
    /// <param name="name">The placeholder value to dynamically replace in the text.</param>
    /// <returns>
    /// A string containing the translated text with the placeholder replaced by the provided value.
    /// </returns>
    public string GetTranslatedText(string name)
    {
        string selectedText = UIExtentions.IsEnglish() ? englishText : greekText;
        return selectedText.Replace("{name}", name); // Replace placeholder with the dynamic value
    }  
 
}
public static class TextLibrary
{
    public static TextClassTranslate GpsText { get; } = new TextClassTranslate(
        "Υπάρχει προσωρινή απόκλιση στο GPS. Βεβαιωθείτε οτι βρίσκεστε σε ανοιχτό χώρο, μακριά από δέντρα ή κτίρια.",
        "There is a temporary deviation in the GPS. Make sure you are in an open area, away from trees or buildings."
    );
    public static TextClassTranslate InRadiusText { get; } = new TextClassTranslate(
        "Φτάσατε στο σημείο {name}. Πλησιάστε για να αντλήσετε σχετικές πληροφορίες",
        "You have reached the {name}. Approach to get more information"
    );
    public static TextClassTranslate arrowText { get; } = new TextClassTranslate(
    "Στρέψτε την κάμερα του κινητού σας για να δείτε τα διαθέσιμα σημεία ενδιαφέροντος",
    "Point the camera of your device around you to see the available points of interest"
    );
    public static TextClassTranslate pointTextTranslate { get; } = new TextClassTranslate( "Σημεία","Points");
    public static TextClassTranslate minutesTextTranslate { get; } = new TextClassTranslate( "λεπτά", "minutes");
    public static TextClassTranslate moreTextTranslate { get; } = new TextClassTranslate( "ΠΕΡΙΣΣΟΤΕΡΑ", "MORE");
    public static TextClassTranslate infoPointBannerText { get; } = new TextClassTranslate( 
    "Στρέψτε την κάμερα του κινητού σας προς την εκτυπωμένη αεροφωτογραφία του Ηρακλείου"
    , "Point the camera of your device to the aerial photo of Heraklion");
    public static TextClassTranslate FarAwayText { get; } = new TextClassTranslate( 
    "Δεν υπάρχουν σημεία ενδιαφέροντος σε αυτήν την περιοχή"
    , "There are no points of interests in this area");
    public static TextClassTranslate minuteTextTranslate { get; } = new TextClassTranslate( "0 λεπτά", "0 minutes");
    public static TextClassTranslate meterTextTranslate { get; } = new TextClassTranslate( "μ", "m");
}
