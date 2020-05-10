namespace LightStreet.Models.PredictionModels
{
    public class Prediction
    {
        public string tagName { get; set; }

        public double probability { get; set; }

        public BoundingBox boundingBox { get; set; }

    }
}
