# ReputationCooldowns
 A Rocket plugin for Unturned which modifies permission cooldowns based on players' reputations.
 The purpose of this plugin is to give a method to discourage KOS and other villainous activities.
 
## Permissions
The only permission within this plugin is `repcooldown.exempt` which exempts players with the permission from the reputation cooldown multipliers.

## Configuration

Default Configuration File:
```
<?xml version="1.0" encoding="utf-8"?>
<Configuration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <RepCooldowns>
    <Range Max="-10">
      <Multiplier>1.5</Multiplier>
    </Range>
    <Range Min="-10" Max="10">
      <Multiplier>1</Multiplier>
    </Range>
    <Range Min="10">
      <Multiplier>0.5</Multiplier>
    </Range>
  </RepCooldowns>
</Configuration>
```

The configuration is made up of a list of reputation ranges with a multipler per each. Each range has three attributes:

- Min - minimum reputation (inclusive), if this is undefined, it acts as negative infinity
- Max - maximum reputation (exclusive), if this is undefined, it acts as positive infinity
- Multiplier - multiplier of the permission cooldown for this range


With this default configuration, players with a reputation lower than -10 will need have a 50% higher cooldown for commands.
