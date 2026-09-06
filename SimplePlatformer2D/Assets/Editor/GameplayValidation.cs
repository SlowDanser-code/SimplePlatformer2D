using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

// Runs against the real scene in Play Mode. Changes disappear when Play Mode exits.
[InitializeOnLoad]
public static class GameplayValidation
{
    private const string _runningKey = "GameplayValidation.Running";
    private const string _failedKey = "GameplayValidation.Failed";
    private static IEnumerator _checks;
    private static int _lastFrame = -1;
    private static double _deadline;

    static GameplayValidation()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    [MenuItem("Tools/Validate Gameplay")]
    public static void Run()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            throw new InvalidOperationException("Stop Play Mode before validation.");

        if (EditorSceneManager.GetActiveScene().isDirty)
            throw new InvalidOperationException("Save your scene before validation.");

        EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity");
        SessionState.SetBool(_runningKey, true);
        SessionState.SetBool(_failedKey, false);
        EditorApplication.EnterPlaymode();
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (SessionState.GetBool(_runningKey, false) == false)
            return;

        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            _checks = CheckGameplay();
            _deadline = EditorApplication.timeSinceStartup + 60;
            Application.logMessageReceived += OnLog;
            EditorApplication.update += Tick;
        }
        else if (state == PlayModeStateChange.EnteredEditMode)
        {
            SessionState.SetBool(_runningKey, false);
            bool failed = SessionState.GetBool(_failedKey, false);
            if (Application.isBatchMode)
                EditorApplication.Exit(failed ? 1 : 0);
        }
    }

    private static void OnLog(string message, string stack, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
            SessionState.SetBool(_failedKey, true);
    }

    private static void Tick()
    {
        try
        {
            if (EditorApplication.timeSinceStartup > _deadline)
                throw new Exception("Gameplay validation timed out.");

            if (_lastFrame == Time.frameCount)
                return;

            _lastFrame = Time.frameCount;
            if (_checks.MoveNext())
                return;

            Finish();
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            SessionState.SetBool(_failedKey, true);
            Finish();
        }
    }

    private static void Finish()
    {
        EditorApplication.update -= Tick;
        Application.logMessageReceived -= OnLog;
        Debug.Log(SessionState.GetBool(_failedKey, false)
            ? "GAMEPLAY VALIDATION FAILED"
            : "GAMEPLAY VALIDATION PASSED");
        EditorApplication.ExitPlaymode();
    }

    private static void Check(bool condition, string message)
    {
        if (condition == false)
            throw new Exception(message);

        Debug.Log("PASS: " + message);
    }

    private static void Place(Rigidbody2D body, Vector2 position)
    {
        body.position = position;
        body.transform.position = position;
        body.velocity = Vector2.zero;
        Physics2D.SyncTransforms();
    }

    private static IEnumerator CheckGameplay()
    {
        yield return null;
        PlayerMovement player = Object.FindObjectOfType<PlayerMovement>();
        Health playerHealth = player.GetComponent<Health>();
        Rigidbody2D playerBody = player.GetComponent<Rigidbody2D>();
        Attack playerAttack = player.GetComponent<Attack>();
        EnemyAI[] enemies = Object.FindObjectsOfType<EnemyAI>();
        Medkit[] medkits = Object.FindObjectsOfType<Medkit>();

        Check(enemies.Length == 2, "Scene contains two enemies.");
        Check(enemies.All(e =>
        {
            SerializedObject patrol = new SerializedObject(e.GetComponent<EnemyPatrol>());
            return patrol.FindProperty("_leftPoint").objectReferenceValue != null &&
                   patrol.FindProperty("_rightPoint").objectReferenceValue != null;
        }), "Patrol waypoint overrides survive serialized field renaming.");
        Check(player.GetComponent<TargetScanner>() != null &&
              enemies.All(e => e.GetComponents<TargetScanner>().Length == 1),
            "Player and enemies have the shared scanner, with one instance per enemy.");
        Check(medkits.Length == 3, "Scene spawns three medkits.");
        Check(medkits.Select(m => m.transform.position).Distinct().Count() == 3,
            "Medkits occupy distinct spawn points.");
        Check(medkits.All(m => Mathf.Abs(m.transform.position.x) <= 8 && m.transform.position.y == 0),
            "Medkits spawn above the level platform.");
        Check(playerHealth.CurrentValue == playerHealth.MaxValue, "Player starts at full health.");

        player.GetComponent<PlayerInputReader>().enabled = false;
        player.enabled = false;
        playerBody.gravityScale = 0;
        Place(playerBody, new Vector2(0, 20));
        CheckFacing(player.transform, direction => player.SendMessage("OnMovementInputChanged", direction));
        player.Stop();
        CheckFacing(enemies[0].transform, enemies[0].GetComponent<EntityRotator>().Rotate);
        CheckScanner();
        float startX = enemies[0].transform.position.x;
        float until = Time.time + 1f;
        while (Time.time < until)
            yield return null;

        Check(Mathf.Abs(enemies[0].transform.position.x - startX) > 0.2f,
            "Enemy patrols when player is out of sight.");
        Check(enemies.All(e => e.transform.position.y > -1), "Patrolling enemies stay on the platform.");

        EnemyAI enemy = enemies[0];
        EnemyDetector detector = enemy.GetComponent<EnemyDetector>();
        Rigidbody2D enemyBody = enemy.GetComponent<Rigidbody2D>();
        Health enemyHealth = enemy.GetComponent<Health>();
        Attack enemyAttack = enemy.GetComponent<Attack>();
        enemies[1].gameObject.SetActive(false);
        Place(enemyBody, new Vector2(-2, 0.02f));
        Place(playerBody, new Vector2(0, 0.05f));
        Check(detector.TryGetPlayer(out Transform target) && target == player.transform,
            "Enemy detects a living player nearby.");
        until = Time.time + 0.15f;
        while (Time.time < until)
            yield return null;
        Check(enemyBody.velocity.x > 0, "Enemy pursues the visible player.");
        Check(enemy.transform.localScale.x > 0, "Enemy movement turns its Transform toward the player.");

        Place(playerBody, new Vector2(0, 20));
        Place(enemyBody, new Vector2(0, 0.02f));
        until = Time.time + 0.15f;
        while (Time.time < until)
            yield return null;
        float patrolX = enemy.GetComponent<EnemyPatrol>().GetTargetPositionX();
        Check(detector.TryGetPlayer(out _) == false && enemyBody.velocity.x * (patrolX - enemy.transform.position.x) > 0,
            "Enemy returns toward its patrol route after losing the player.");
        Check(Mathf.Sign(enemy.transform.localScale.x) == Mathf.Sign(enemyBody.velocity.x),
            "Enemy view keeps the hierarchy facing its patrol direction.");

        enemy.enabled = false;
        enemyBody.gravityScale = 0;
        Place(enemyBody, new Vector2(0, 10));
        Place(playerBody, new Vector2(2, 10));
        Check(playerAttack.TryAttack() == false, "Player cannot attack outside melee range.");

        Place(playerBody, new Vector2(0.8f, 10));
        GameObject wall = new GameObject("Validation wall") { layer = LayerMask.NameToLayer("Ground") };
        wall.transform.position = new Vector3(0.4f, 10);
        wall.AddComponent<BoxCollider2D>().size = new Vector2(0.1f, 2);
        Physics2D.SyncTransforms();
        Check(detector.TryGetPlayer(out _) == false, "A wall blocks enemy vision.");
        Check(playerAttack.TryAttack() == false, "A wall blocks melee damage.");
        Object.Destroy(wall);
        yield return null;
        Physics2D.SyncTransforms();

        Check(playerAttack.TryAttack(), "Player can hit a nearby enemy.");
        Check(enemyHealth.CurrentValue == 75, "Player attack deals 25 damage.");
        Check(playerAttack.TryAttack() == false && enemyHealth.CurrentValue == 75,
            "Cooldown prevents repeated damage in the same frame.");
        Check(enemyAttack.TryAttack() && playerHealth.CurrentValue == 90,
            "Enemy can deal 10 damage to the player.");
        until = Time.time + 0.55f;
        while (Time.time < until)
            yield return null;
        Check(playerAttack.TryAttack() && enemyHealth.CurrentValue == 50,
            "Player can attack again after cooldown.");

        Place(enemyBody, new Vector2(0, 0.02f));
        Place(playerBody, new Vector2(0.8f, 0.05f));
        enemy.enabled = true;
        float previousHealth = playerHealth.CurrentValue;
        until = Time.time + 1.1f;
        while (Time.time < until)
            yield return null;
        Check(playerHealth.CurrentValue < previousHealth && Mathf.Abs(enemyBody.velocity.x) < 0.01f,
            "Enemy AI stops and attacks automatically in melee range.");
        enemy.enabled = false;

        Place(enemyBody, new Vector2(9.6f, 0.02f));
        enemy.GetComponent<EnemyMovement>().MoveTo(12);
        Check(enemyBody.velocity.x == 0, "Enemy refuses to walk off the platform edge.");
        Place(enemyBody, new Vector2(-9.6f, 0.02f));
        enemy.GetComponent<EnemyMovement>().MoveTo(-12);
        Check(enemyBody.velocity.x == 0, "Enemy is also protected at the left platform edge.");

        playerHealth.TryHeal(1000);
        Place(playerBody, medkits[0].transform.position);
        until = Time.time + 0.1f;
        while (Time.time < until)
            yield return null;
        Check(medkits[0].gameObject.activeSelf, "Full health does not consume a medkit.");
        playerHealth.TakeDamage(10);
        until = Time.time + 0.1f;
        while (Time.time < until)
            yield return null;
        Check(playerHealth.CurrentValue == 100 && medkits[0].gameObject.activeSelf == false,
            "An overlapping medkit heals after damage and caps health at maximum.");
        playerHealth.TakeDamage(40);
        Check(medkits[0].TryUse(playerHealth) == false && playerHealth.CurrentValue == 60,
            "A consumed medkit cannot be reused.");
        Check(medkits[1].TryUse(playerHealth) && playerHealth.CurrentValue == 85,
            "A fresh medkit restores 25 health.");

        // Re-enable actual controls so the death handler, rather than test setup, disables them.
        player.enabled = true;
        player.GetComponent<PlayerInputReader>().enabled = true;
        Place(playerBody, new Vector2(-8.6f, 0.05f));
        int deathEvents = 0;
        playerHealth.Died += () => deathEvents++;
        playerHealth.TakeDamage(1000);
        playerHealth.TakeDamage(1000);
        Check(playerHealth.CurrentValue == 0 && deathEvents == 1, "Death fires once and health never becomes negative.");
        Check(player.enabled == false && player.GetComponent<PlayerInputReader>().enabled == false &&
              player.GetComponent<PlayerAttackController>().enabled == false,
            "Death disables movement, input and player attack controls.");
        Check(detector.TryGetPlayer(out _) == false, "Enemy ignores the dead player in detection range.");
        Check(playerAttack.TryAttack() == false && playerHealth.TryHeal(25) == false,
            "Dead player cannot attack or heal.");
        enemyHealth.TakeDamage(1000);
        Check(enemy.gameObject.activeSelf == false, "Dead enemy is removed from gameplay.");

        // Check that standalone prefab instances also have a safe patrol without scene waypoints.
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemy.prefab");
        GameObject instance = Object.Instantiate(prefab, new Vector3(0, 10), Quaternion.identity);
        Check(instance.GetComponent<EnemyPatrol>().GetTargetPositionX() > instance.transform.position.x,
            "Enemy prefab patrol works without manually assigned waypoints.");
        Object.Destroy(instance);
        yield return null;
    }

    private static void CheckFacing(Transform entity, Action<float> turn)
    {
        Vector3 originalScale = entity.localScale;
        entity.localScale = new Vector3(1.5f, 2, 1);
        GameObject probe = new GameObject("Validation child trigger");
        probe.transform.SetParent(entity, false);
        probe.transform.localPosition = new Vector3(0.75f, 0.25f, 0);
        BoxCollider2D trigger = probe.AddComponent<BoxCollider2D>();
        trigger.isTrigger = true;
        trigger.size = new Vector2(0.1f, 0.1f);

        turn(-1);
        Physics2D.SyncTransforms();
        Check(probe.transform.position.x < entity.position.x && trigger.bounds.center.x < entity.position.x,
            entity.name + ": facing left also moves the child Transform and trigger to the left.");
        turn(0);
        Check(entity.localScale.x == -1.5f, entity.name + ": stopping preserves the facing direction.");
        turn(1);
        Physics2D.SyncTransforms();
        Check(probe.transform.position.x > entity.position.x && trigger.bounds.center.x > entity.position.x &&
              entity.localScale == new Vector3(1.5f, 2, 1),
            entity.name + ": facing right restores the hierarchy and preserves scale magnitudes.");

        Object.Destroy(probe);
        entity.localScale = originalScale;
        Physics2D.SyncTransforms();
    }

    private static void CheckScanner()
    {
        GameObject owner = new GameObject("Validation scanner");
        owner.transform.position = new Vector3(50, 50, 0);
        TargetScanner scanner = owner.AddComponent<TargetScanner>();
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int playerLayer = LayerMask.NameToLayer("Player");
        Collider2D near = CreateScanTarget(owner.transform, 0.5f, enemyLayer, false);
        CreateScanTarget(owner.transform, 2, enemyLayer, false);
        Collider2D otherLayer = CreateScanTarget(owner.transform, 0.6f, playerLayer, false);
        CreateScanTarget(owner.transform, 0.2f, enemyLayer, true);
        Physics2D.SyncTransforms();

        var targets = scanner.Scan(1, 1 << enemyLayer);
        Check(targets.Count == 1 && targets[0] == near,
            "Scanner respects radius and layer while excluding trigger colliders.");
        Check(scanner.Scan(3, 1 << enemyLayer).Count == 2,
            "The same scanner supports a larger detection radius.");
        targets = scanner.Scan(1, 1 << playerLayer);
        Check(targets.Count == 1 && targets[0] == otherLayer,
            "Changing the scanner layer does not retain results from the previous query.");
        Check(scanner.Scan(0.05f, 1 << enemyLayer).Count == 0,
            "An empty scan clears the previous results.");
        Object.Destroy(owner);
    }

    private static Collider2D CreateScanTarget(Transform parent, float x, int layer, bool isTrigger)
    {
        GameObject target = new GameObject("Validation target") { layer = layer };
        target.transform.SetParent(parent, false);
        target.transform.localPosition = new Vector3(x, 0, 0);
        BoxCollider2D collider = target.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(0.1f, 0.1f);
        collider.isTrigger = isTrigger;
        return collider;
    }
}
