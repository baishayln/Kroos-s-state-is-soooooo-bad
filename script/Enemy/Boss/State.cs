public interface State
{
    
    void OnEnter();

    void OnUpdate();
    
    void OnFixedUpdate();

    void OnExit();

    // void SetDate();
}
