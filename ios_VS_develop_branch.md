# branch-ios vs develop — Feature Comparison

**Project:** twinbros (Unity C# game)  
**Date:** 2026-07-29

---

## Executive Summary

`branch-ios` and `develop` are very close — they share the same core architecture (PlatformManager, LeaderboardsManager, Achievements, MyStoreClass). The key difference is **ads**: `develop` has an active GameDistribution ad integration, while `branch-ios` has all ads entirely commented out.

---

## Summary Table

| Feature | branch-ios | develop |
|---|---|---|
| **IAP** | ✅ Active (Unity IAP, `#if` gated to mobile) | ✅ Same (identical file) |
| **Ads (Google AdMob)** | ❌ Entire file commented out | ❌ Active but `#if` gated to mobile (won't fire on WebGL) |
| **Ads (GameDistribution web)** | ❌ Entire file commented out | ✅ **ACTIVE** — `GameDistribution.cs` + `GameDistributionAds.cs` |
| **GUIManager ad ref** | ❌ Commented out | ✅ `GameDistributionAds adsScript` (active) |
| **Leaderboards** | ✅ Unity Gaming Services (`LeaderboardsManager.cs`) | ✅ Same (identical file) |
| **Achievements** | ✅ `Achievements.cs` (local) + `SocialAPI.cs` | ✅ Same (identical files) |
| **PlatformManager** | ✅ Sophisticated (isWebVersion, isArcadeOrSubscriptionMode, etc.) | ✅ Same (identical file) |
| **GameConstants** | ✅ Full — LEADERBOARD_ID, achievements, tutorial, NUM_STAGES | ✅ Same (identical file) |
| **GameDistributionAds.cs** | ❌ Does not exist | ✅ Exists — extends GenericAdsManager, GameDistribution SDK wrapper |
| **GenericAdsManager.cs** | ✅ Present (abstract base) | ✅ Same (identical file) |

---

## Detailed Differences

### 📢 Ads — THE KEY DIFFERENCE

| File | branch-ios | develop |
|---|---|---|
| `GoogleMobileAdsScript.cs` | **Entirely commented out** (lines 1-268 inside `/* ... */`) | **Active**, `#if UNITY_ANDROID \|\| UNITY_IPHONE` gated |
| `GameDistribution.cs` | **Entirely commented out** (`/* ... */`) | **Active** — GameDistribution web SDK with `[DllImport("__Internal")]` JS interop |
| `GameDistributionAds.cs` | **Does not exist** | **Active** — extends `GenericAdsManager`, wires GameDistribution events |
| `GUIManager.cs` ad reference (line 99-100) | `//GoogleMobileAdsScript adsScript;` (commented) + `//GameDistributionAds adsScript;` (commented) | `//GoogleMobileAdsScript adsScript;` (commented) + `GameDistributionAds adsScript;` (active!) |
| `GUIManager.cs` Start() | `//adsScript = scripts.GetComponent<GameDistributionAds>();` (commented) | `adsScript = scripts.GetComponent<GameDistributionAds>();` (active!) |

#### What this means:

On **develop**, when the game runs:
1. `GameDistributionAds` is instantiated and subscribes to `GameDistribution` events
2. `GameDistribution` initializes the SDK (calls JS interop `SDK_Init()`)
3. If running on WebGL in a browser **with GameDistribution's HTML5 SDK loaded**, ads **WILL show**
4. If running on WebGL without the SDK, SDK calls fail silently (`EntryPointNotFoundException` → `Debug.LogWarning`)
5. If running on mobile, GameDistribution JS interop won't work but Google AdMob code is `#if` gated — so no ads there either (unless you're on Android/iOS where the `#if` would activate)
6. `GameDistribution` is specifically designed for WebGL HTML5 game portals — it requires their JavaScript SDK loaded in the browser

#### The Issue:

`develop` is **not ad-free**. The `GameDistributionAds` / `GameDistribution` integration is wired up and active. If deployed to a GameDistribution portal (or any page loading their JS SDK), ads **will** show. The game doesn't check `PlatformManager.isWebVersion()` or `isArcadeOrSubscriptionMode` to gate these ads.

---

### 🛒 IAP — Identical

Both branches have the exact same `MyStoreClass.cs`:
- Gated with `#if UNITY_ANDROID || UNITY_IPHONE`
- 3 products: `product_extra_moves`, `product_infinite_revives`, `product_remove_ads`
- iOS restore via `IAppleExtensions`
- On WebGL builds, the entire class is compiled away ✅

---

### 🏆 Leaderboards — Identical

Both have the same `LeaderboardsManager.cs` using Unity Gaming Services:
- `Unity.Services.Leaderboards` + `Unity.Services.Authentication`
- Leaderboard ID: `"twins_high_cores"`
- Supports WebGL, iOS, Android
- `PlatformManager.IsLeaderboardsSupportingPlatform()` → WebGLPlayer || IPhonePlayer || Android || test

---

### 🎖️ Achievements — Identical

Both have:
- `Achievements.cs` — Local PlayerPrefs-based stage/level tracking
- `SocialAPI.cs` — GameCenter (iOS) / Google Play (Android) via `UnityEngine.SocialPlatforms`
- Achievement IDs: `grp.twins_stage_1` through `grp.twins_stage_5`

---

### 🧠 PlatformManager — Identical

```
isArcadeOrSubscriptionMode → Linux/Windows/OSX Player
isWebVersion() → WebGLPlayer
isEditorVersion() → Linux/OSX/Windows Editor
hasInAppPurchasesSupport() → IsMobilePlatform() || test
IsAdsSupportingPlatform() → (LinuxEditor || WebGLPlayer) && canShowAds
IsLeaderboardsSupportingPlatform() → WebGL || iOS || Android || test
IsPurchasesSupportingPlatform() → iOS || Android || test
canShowAds flag → false by default
```

---

## Action Plan: Making `develop` the WebGL Branch

If `develop` should replace `branch-webgl` as the **ad-free, IAP-free WebGL branch** with leaderboards and achievements, here's what needs to change:

### ✅ Already correct:
- IAP is `#if` gated — won't compile on WebGL
- Leaderboards (UGS) works on WebGL
- Achievements work (local PlayerPrefs)
- PlatformManager can distinguish WebGL via `isWebVersion()`
- `canShowAds` defaults to `false`

### ❌ Needs fixing for ad-free WebGL:

1. **GameDistributionAds.cs + GameDistribution.cs** — either:
   - Comment out entirely (like branch-ios does), OR
   - Add `isArcadeOrSubscriptionMode` / `isWebVersion()` gate to prevent loading on WebGL

2. **GUIManager.cs** — Change ad reference:
   ```
   // GameDistributionAds adsScript;      // comment out
   //GenericAdsManager adsScript;        // or use abstract type, initialised only when not web
   ```
   And in `Start()`:
   ```
   // adsScript = scripts.GetComponent<GameDistributionAds>();  // comment out
   ```
   OR gate it:
   ```
   if (!platformManager.isWebVersion() && !platformManager.isArcadeOrSubscriptionMode) {
       adsScript = scripts.GetComponent<GameDistributionAds>();
   }
   ```

3. **GoogleMobileAdsScript.cs** — Already `#if` gated, but could be commented out for cleanliness (like branch-ios)

### Recommended approach:
Since `branch-ios` already has ads completely stripped (commented out), the cleanest path is to **copy the ad-related changes from branch-ios into develop**:
- Comment out `GoogleMobileAdsScript.cs` entirely
- Comment out `GameDistribution.cs` entirely
- Comment out `GameDistributionAds.cs` references in `GUIManager.cs`
- Keep everything else as-is (leaderboards, achievements, IAP gating)

This gives you a **clean WebGL branch** with:
- ❌ No ads
- ❌ No IAP (compile-gated)
- ✅ Unity Gaming Services leaderboards
- ✅ Local + platform achievements
- ✅ Same game logic as iOS

---

## Three-Way Architecture Comparison

| Aspect | branch-ios | develop | branch-webgl |
|---|---|---|---|
| AdMob | Commented out | Active (#if gated) | Active (#if gated) |
| GameDistribution | Commented out | **ACTIVE** | Deleted |
| GameDistributionAds | Doesn't exist | **ACTIVE** | Doesn't exist |
| GenericAdsManager | Present | Present | Deleted |
| GUIManager ad ref | Commented out | **GameDistributionAds** | GoogleMobileAdsScript |
| IAP | #if gated ✅ | #if gated ✅ | #if gated ✅ |
| Leaderboards (UGS) | ✅ | ✅ | ❌ (missing) |
| Achievements (local) | ✅ | ✅ | ❌ (missing) |
| PlatformManager | Sophisticated | Sophisticated | Simple/flags |
| **Ad-free guarantee** | ✅ Safe | ⚠️ **NOT safe** | ✅ Safe (runtime) |
| **WebGL fit** | Needs IAP stripping | Needs ad stripping | Missing features |
