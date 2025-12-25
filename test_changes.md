# SessionReward Changes Summary

## Changes Made:

1. **SessionRewardEntity.cs**:
   - Changed `SessionRewardId` to `Id`
   - Removed `RewardId` property
   - Maintained one-to-one relationship with RewardEntity

2. **SessionRewardEntityConfiguration.cs**:
   - Updated primary key to use `Id`
   - Set `ValueGeneratedNever()` since ID comes from RewardEntity
   - Updated foreign key relationship to use `Id`

3. **SessionRewards.cs**:
   - Updated `SessionRewardId` property to return `SessionRewardEntity.Id`
   - Updated `Save()` method to set `SessionRewardEntity.Id = this.RewardId`
   - Updated queries to use `RewardId` instead of `SessionRewardId`

4. **SessionRewardFactory.cs**:
   - Updated `FindAsync` to use `Id` instead of `SessionRewardId`
   - Updated `FindByRewardIdAsync` to use `Id` instead of `RewardId`

5. **DeleteSessionsReward.cs**:
   - Updated queries to use `Id` instead of `SessionRewardId`
   - Simplified delete logic for one-to-one relationship

6. **GetAllSessionsRewards.cs**:
   - Updated DTO creation to use `Id` instead of `SessionRewardId`

7. **Test Files**:
   - Updated test assertions and method calls to use new structure

## Result:
- SessionReward now uses the same ID as its corresponding Reward
- One-to-one relationship established between SessionReward and Reward
- All related code updated to work with the new structure