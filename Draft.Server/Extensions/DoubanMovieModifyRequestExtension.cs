using Draft.Models;
using Draft.Models.Dto.Movie;

namespace Draft.Server.Extensions;

internal static class DoubanMovieModifyRequestExtension {
    public static DoubanMovie ToModel(this DoubanMovieModifyRequest request)
        => new(request.Title) {
            OtherTitles  = request.OtherTitles ?? [],
            StaffInfos   = request.StaffInfos ?? "",
            Year         = request.Year ?? "",
            Region       = request.Region ?? "",
            Tags         = request.Tags ?? [],
            Rating       = request.Rating ?? 0,
            RatingCount  = request.RatingCount ?? 0,
            Rank         = request.Rank ?? 0,
            Quote        = request.Quote ?? "",
            Url          = request.Url ?? "",
            PreviewImage = request.PreviewImage ?? "",
            Favorites    = []
        };

    public static DoubanMovie Modify(this DoubanMovieModifyRequest request, DoubanMovie model) {
        if (request.OtherTitles != null) model.OtherTitles   = request.OtherTitles;
        if (request.StaffInfos != null) model.StaffInfos     = request.StaffInfos;
        if (request.Year != null) model.Year                 = request.Year;
        if (request.Region != null) model.Region             = request.Region;
        if (request.Tags != null) model.Tags                 = request.Tags;
        if (request.Rating != null) model.Rating             = request.Rating.Value;
        if (request.RatingCount != null) model.RatingCount   = request.RatingCount.Value;
        if (request.Rank != null) model.Rank                 = request.Rank.Value;
        if (request.Quote != null) model.Quote               = request.Quote;
        if (request.Url != null) model.Url                   = request.Url;
        if (request.PreviewImage != null) model.PreviewImage = request.PreviewImage;
        return model;
    }
}
