# branch-ios vs branch-webgl — Feature Comparison

**Project:** twinbros (Unity C# game)  
**Date:** 2026-07-29

---

## Summary Table

| Feature | branch-ios | branch-webgl |
|---|---|---|
| **IAP** | ✅ Active (Unity IAP, 3 products) — `#if` gated to mobile | ❌ Not compiled (same `#if` gate) — but PlatformManager lacks proper checks |
| **Ads (AdMob)** | ❌ Entire file commented out | ❌ Not compiled (`#if` gate) — but code is active & messy in GUIManager |
| **Ads (GameDistribution)** | ❌ Entire file commented out | ❌ Folder deleted entirely |
| **Leaderboards** | ✅ Unity Gaming Services (`LeaderboardsManager.cs`) | ❌ Missing — only classic `SocialAPI.cs` |
| **Achievements** | ✅ Local (`Achievements.cs`) + GameCenter/Play (`SocialAPI.cs`) | ❌ Missing `Achievements.cs` — only `SocialAPI.cs` |
| **GenericAdsManager** | ✅ Abstract base class present | ❌ Deleted |
| **PlatformManager** | ✅ Sophisticated: `isWebVersion()`, `isArcadeOrSubscriptionMode`, `hasInAppPurchasesSupport()`, `canShowAds` | ❌ Simple: boolean flags only, no runtime platform checks |

---

## branch-ios — Detailed Findings

### 🛒 In-App Purchases — **YES, active**

| File | Details |
|---|---|
| `MyStoreClass.cs` | Unity IAP (`UnityEngine.Purchasing`), entire class gated with `#if UNITY_ANDROID || UNITY_IPHONE` |
| `MyIAPListener.cs` | Thin wrapper, same `#if` gate |
| Products | 3 non-consumables: `product_extra_moves`, `product_infinite_revives`, `product_remove_ads` |
| iOS restore | `IAppleExtensions.RestoreTransactions` |
| Gate | `PlatformManager.hasInAppPurchasesSupport()` → `IsMobilePlatform()` only |
| Config | `Assets/Resources/BillingMode.json`: `{"androidStore":"GooglePlay"}` |
| Purchase checks | `GameManagerScript.HasPurchased*()` methods via `PlayerPrefs.GetInt(PRODUCT_*, 0) == 1` |
| GUI integration | `GUIManager.cs` shows purchase buttons, prices, restore button (iOS only) |

### 📢 Ads — **DISABLED (code commented out)**

| File | Status |
|---|---|
| `GoogleMobileAdsScript.cs` | **Entire file commented out** (`/* ... */`) |
| `GameDistribution.cs` | **Entire file commented out** (`/* ... */`) |
| `GenericAdsManager.cs` | Exists as abstract base class, but unused |
| `PlatformManager.canShowAds` | `false` by default → ads disabled |
| `PlatformManager.IsAdsSupportingPlatform()` | Returns true for `(LinuxEditor || WebGLPlayer) && canShowAds` |
| GUI | Most ad code in `GUIManager.cs` is commented out |

### 🏆 Leaderboards — **YES (Unity Gaming Services)**

- **File:** `LeaderboardsManager.cs`
- **SDK:** `Unity.Services.Leaderboards` + `Unity.Services.Authentication`
- **Leaderboard ID:** `"twins_high_cores"` (web-oriented; iOS ID `grp.twins_high_cores` is commented out)
- **Platform support:** `IsLeaderboardsSupportingPlatform()` → WebGL, iOS, Android, or test

### 🎖️ Achievements — **YES (local + platform)**

- **`Achievements.cs`** — Local PlayerPrefs-based stage/level completion tracking
  - Uses `ACHIEVEMENT_STAGE_LEVEL_STR = "achievement_stage_{0}_level_{1}"`
  - `CheckAchievements()` reads PlayerPrefs to determine stage progress
- **`SocialAPI.cs`** — GameCenter (iOS) / Google Play (Android) via `UnityEngine.SocialPlatforms`
  - `AddAchievement(id, progress)` — platform achievements
  - `ReportScore(score, leaderboardId)` — platform leaderboards
  - `ShowLeaderBoards()` / `ShowAchievements()` — platform UI

### 🧠 PlatformManager — Sophisticated

```
isArcadeOrSubscriptionMode → Linux/Windows/OSX Player
isWebVersion() → WebGLPlayer
isEditorVersion() → Linux/OSX/Windows Editor
hasInAppPurchasesSupport() → IsMobilePlatform() || test
IsAdsSupportingPlatform() → (LinuxEditor || WebGLPlayer) && canShowAds
IsLeaderboardsSupportingPlatform() → WebGL || iOS || Android || test
IsPurchasesSupportingPlatform() → iOS || Android || test
```

---

## branch-webgl — Detailed Findings

### 🛒 IAP — **Effectively no** (compiled out)

- Same `#if UNITY_ANDROID || UNITY_IPHONE` gating on `MyStoreClass.cs` → code stripped on WebGL builds ✅
- `GUIManager.cs` references `store` only inside `#if UNITY_ANDROID || UNITY_IPHONE` ✅
- **Problem:** `PlatformManager` is simpler — **no** `hasInAppPurchasesSupport()`, **no** `isWebVersion()`. `IsPurchasesSupportingPlatform()` just checks boolean flags, not runtime platform.
- `GameConstants.cs` doesn't have `ACHIEVEMENT_STAGE_LEVEL_STR`, `CURRENT_STAGE_OR_LEVEL`, `NUM_STAGES`, `NUM_LEVELS_PER_STAGE`

### 📢 Ads — **Architecturally messy but won't fire at runtime**

| Issue | Detail |
|---|---|
| `GoogleMobileAdsScript.cs` | **ACTIVE** (not commented out), but all Google APIs inside `#if UNITY_ANDROID || UNITY_IPHONE` |
| `GenericAdsManager.cs` | **MISSING** (deleted entirely) |
| `GameDistribution/` | **MISSING** (deleted entirely) |
| `GUIManager.cs` | References `GoogleMobileAdsScript` directly, **ad interstitial/reward code is active** (lines 739-758) |
| `PlatformManager.IsAdsSupportingPlatform()` | Returns `!isMacOS && !isHuaweiAndroid` — **too broad**, doesn't exclude WebGL |

While ads **won't actually run** on WebGL at runtime (GoogleMobileAds API calls behind `#if` guards return `false`), the architecture is messy:
- The ad script is still active (not commented out like on iOS)
- `GUIManager` has live ad interstitial/reward video logic wired to `GoogleMobileAdsScript`
- `PlatformManager` doesn't properly distinguish WebGL platform
- `isArcadeOrSubscriptionMode` is a public bool on `GUIManager` itself instead of being in `PlatformManager`

### 🏆 Leaderboards — **NO** (Unity Gaming Services missing)

- `LeaderboardsManager.cs` **does not exist**
- Only `SocialAPI.cs` remains (classic `Social.ShowLeaderboardUI` via `UnityEngine.SocialPlatforms`)
- `GameConstants.LEADERBOARD_ID = "grp.twins_high_cores"` (iOS/Android ID)
- `PlatformManager.IsLeaderboardsSupportingPlatform()` → `!isHuaweiAndroid`

### 🎖️ Achievements — **NO**

- `Achievements.cs` **does not exist**
- Only `SocialAPI.cs` remains for platform achievements (GameCenter/Google Play only, no local tracking)

### 🧠 PlatformManager — Simple

```
Simple boolean flags: isGooglePlayAndroid, isHuaweiAndroid, isIOS, isMacOS
IsAdsSupportingPlatform() → !isMacOS && !isHuaweiAndroid
IsLeaderboardsSupportingPlatform() → !isHuaweiAndroid
IsPurchasesSupportingPlatform() → !isMacOS && !isHuaweiAndroid
NO isWebVersion(), NO isArcadeOrSubscriptionMode (on GUIManager instead), NO hasInAppPurchasesSupport()
```

---

## Key Architectural Differences

| Aspect | branch-ios (better) | branch-webgl (worse) |
|---|---|---|
| Ad abstraction | `GenericAdsManager` abstract class | Direct `GoogleMobileAdsScript` reference |
| Platform detection | `PlatformManager` with runtime checks | Simple boolean flags, no runtime awareness |
| WebGL awareness | `isWebVersion()` method | No WebGL detection at all |
| Arcade/Subscription mode | `PlatformManager.isArcadeOrSubscriptionMode` | Public bool on `GUIManager` itself |
| Leaderboards | Unity Gaming Services (cross-platform) | Classic Social API only |
| Local achievements | `Achievements.cs` (PlayerPrefs) | Missing entirely |

---

## Conclusion

`branch-webgl` successfully has **no functional IAP or ads** on WebGL at runtime (thanks to `#if` compile guards), but the architecture is significantly behind `branch-ios`:
- Missing Unity Gaming Services leaderboards
- Missing local achievements system
- No `GenericAdsManager` abstraction
- `PlatformManager` is primitive and lacks WebGL awareness
- Ad code is active in GUI layer even though it can't fire

`branch-ios` would be a much better foundation for a WebGL build — it just needs the IAP store to remain `#if`-gated (already done) and the ad scripts to stay commented out (already done), while keeping the richer leaderboards and achievements infrastructure.
