using UnityEngine;

public interface IInteractionPromptProvider
{
    string InteractionText { get; }
    Transform PromptTarget { get; }
}