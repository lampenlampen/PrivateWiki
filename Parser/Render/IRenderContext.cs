namespace Parser.Render
{
    /// <summary>
    ///     Helper for holding persistent state of Renderer.
    /// </summary>
    public interface IRenderContext
	{
        /// <summary>
        ///     Gets or sets the parent object for this Context.
        /// </summary>
        object Parent { get; set; }

        /// <summary>
        ///     Clones the Context.
        /// </summary>
        /// <returns>Clone.</returns>
        IRenderContext Clone();
	}
}