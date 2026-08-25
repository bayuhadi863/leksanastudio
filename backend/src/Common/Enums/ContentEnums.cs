namespace LeksanaStudio.Common.Enums
{
    /// <summary>
    /// Honest labelling, required by blueprint 05 §5.6: work done for someone else
    /// is never presented as the same thing as work done for oneself.
    /// </summary>
    public enum CaseStudyLabel
    {
        Client = 0,
        OwnProduct = 1,
    }

    /// <summary>The three things a technical note is allowed to be.</summary>
    public enum NotePillar
    {
        Decision = 0,
        Guide = 1,
        Industry = 2,
    }

    /// <summary>
    /// How a service is priced. Not decoration — it decides which block the page
    /// renders: a package table, or a sequence of phases.
    /// </summary>
    public enum PricingShape
    {
        BusinessPackages = 0,
        CorporatePackages = 1,
        Phases = 2,
    }

    /// <summary>Which comparison table a package belongs to.</summary>
    public enum PackageGroup
    {
        Business = 0,
        Corporate = 1,
    }

    /// <summary>
    /// The stand-in drawing used until a real screenshot exists. Drawn from design
    /// tokens, so it reads correctly in both colour schemes.
    /// </summary>
    public enum SchematicVariant
    {
        System = 0,
        Website = 1,
        Catalog = 2,
    }
}
