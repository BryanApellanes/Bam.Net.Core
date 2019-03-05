using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A vehicle is a device that is designed or used to transport people or cargo over land, water, air, or through space.</summary>
	public class Vehicle: Product
	{
		///<summary>The time needed to accelerate the vehicle from a given start velocity to a given target velocity.Typical unit code(s): SEC for secondsNote: There are unfortunately no standard unit codes for seconds/0..100 km/h or seconds/0..60 mph. Simply use "SEC" for seconds and indicate the velocities in the name of the QuantitativeValue, or use valueReference with a QuantitativeValue of 0..60 mph or 0..100 km/h to specify the reference speeds.</summary>
		public QuantitativeValue AccelerationTime {get; set;}
		///<summary>Indicates the design and body style of the vehicle (e.g. station wagon, hatchback, etc.).</summary>
		public OneOfThese<QualitativeValue,Text,Url> BodyType {get; set;}
		///<summary>The available volume for cargo or luggage. For automobiles, this is usually the trunk volume.Typical unit code(s): LTR for liters, FTQ for cubic foot/feetNote: You can use minValue and maxValue to indicate ranges.</summary>
		public QuantitativeValue CargoVolume {get; set;}
		///<summary>The date of the first registration of the vehicle with the respective public authorities.</summary>
		public Bam.Net.Schema.Org.DataTypes.Date DateVehicleFirstRegistered {get; set;}
		///<summary>The drive wheel configuration, i.e. which roadwheels will receive torque from the vehicle's engine via the drivetrain.</summary>
		public OneOfThese<DriveWheelConfigurationValue,Text> DriveWheelConfiguration {get; set;}
		///<summary>The CO2 emissions in g/km. When used in combination with a QuantitativeValue, put "g/km" into the unitText property of that value, since there is no UN/CEFACT Common Code for "g/km".</summary>
		public Number EmissionsCO2 {get; set;}
		///<summary>The capacity of the fuel tank or in the case of electric cars, the battery. If there are multiple components for storage, this should indicate the total of all storage of the same type.Typical unit code(s): LTR for liters, GLL of US gallons, GLI for UK / imperial gallons, AMH for ampere-hours (for electrical vehicles).</summary>
		public QuantitativeValue FuelCapacity {get; set;}
		///<summary>The amount of fuel consumed for traveling a particular distance or temporal duration with the given vehicle (e.g. liters per 100 km).Note 1: There are unfortunately no standard unit codes for liters per 100 km.  Use unitText to indicate the unit of measurement, e.g. L/100 km.Note 2: There are two ways of indicating the fuel consumption, fuelConsumption (e.g. 8 liters per 100 km) and fuelEfficiency (e.g. 30 miles per gallon). They are reciprocal.Note 3: Often, the absolute value is useful only when related to driving speed ("at 80 km/h") or usage pattern ("city traffic"). You can use valueReference to link the value for the fuel consumption to another value.</summary>
		public QuantitativeValue FuelConsumption {get; set;}
		///<summary>The distance traveled per unit of fuel used; most commonly miles per gallon (mpg) or kilometers per liter (km/L).Note 1: There are unfortunately no standard unit codes for miles per gallon or kilometers per liter. Use unitText to indicate the unit of measurement, e.g. mpg or km/L.Note 2: There are two ways of indicating the fuel consumption, fuelConsumption (e.g. 8 liters per 100 km) and fuelEfficiency (e.g. 30 miles per gallon). They are reciprocal.Note 3: Often, the absolute value is useful only when related to driving speed ("at 80 km/h") or usage pattern ("city traffic"). You can use valueReference to link the value for the fuel economy to another value.</summary>
		public QuantitativeValue FuelEfficiency {get; set;}
		///<summary>The type of fuel suitable for the engine or engines of the vehicle. If the vehicle has only one engine, this property can be attached directly to the vehicle.</summary>
		public OneOfThese<QualitativeValue,Text,Url> FuelType {get; set;}
		///<summary>A textual description of known damages, both repaired and unrepaired.</summary>
		public Text KnownVehicleDamages {get; set;}
		///<summary>Indicates that the vehicle meets the respective emission standard.</summary>
		public OneOfThese<QualitativeValue,Text,Url> MeetsEmissionStandard {get; set;}
		///<summary>The total distance travelled by the particular vehicle since its initial production, as read from its odometer.Typical unit code(s): KMT for kilometers, SMI for statute miles</summary>
		public QuantitativeValue MileageFromOdometer {get; set;}
		///<summary>The release date of a vehicle model (often used to differentiate versions of the same make and model).</summary>
		public Bam.Net.Schema.Org.DataTypes.Date ModelDate {get; set;}
		///<summary>The number or type of airbags in the vehicle.</summary>
		public OneOfThese<Number,Text> NumberOfAirbags {get; set;}
		///<summary>The number of axles.Typical unit code(s): C62</summary>
		public OneOfThese<Number,QuantitativeValue> NumberOfAxles {get; set;}
		///<summary>The number of doors.Typical unit code(s): C62</summary>
		public OneOfThese<Number,QuantitativeValue> NumberOfDoors {get; set;}
		///<summary>The total number of forward gears available for the transmission system of the vehicle.Typical unit code(s): C62</summary>
		public OneOfThese<Number,QuantitativeValue> NumberOfForwardGears {get; set;}
		///<summary>The number of owners of the vehicle, including the current one.Typical unit code(s): C62</summary>
		public OneOfThese<Number,QuantitativeValue> NumberOfPreviousOwners {get; set;}
		///<summary>The permitted weight of passengers and cargo, EXCLUDING the weight of the empty vehicle.Typical unit code(s): KGM for kilogram, LBR for poundNote 1: Many databases specify the permitted TOTAL weight instead, which is the sum of weight and payloadNote 2: You can indicate additional information in the name of the QuantitativeValue node.Note 3: You may also link to a QualitativeValue node that provides additional information using valueReference.Note 4: Note that you can use minValue and maxValue to indicate ranges.</summary>
		public QuantitativeValue Payload {get; set;}
		///<summary>The date of production of the item, e.g. vehicle.</summary>
		public Bam.Net.Schema.Org.DataTypes.Date ProductionDate {get; set;}
		///<summary>The date the item e.g. vehicle was purchased by the current owner.</summary>
		public Bam.Net.Schema.Org.DataTypes.Date PurchaseDate {get; set;}
		///<summary>The number of persons that can be seated (e.g. in a vehicle), both in terms of the physical space available, and in terms of limitations set by law.Typical unit code(s): C62 for persons</summary>
		public OneOfThese<Number,QuantitativeValue> SeatingCapacity {get; set;}
		///<summary>The speed range of the vehicle. If the vehicle is powered by an engine, the upper limit of the speed range (indicated by maxValue should be the maximum speed achievable under regular conditions.Typical unit code(s): KMH for km/h, HM for mile per hour (0.447 04 m/s), KNT for knot*Note 1: Use minValue and maxValue to indicate the range. Typically, the minimal value is zero.* Note 2: There are many different ways of measuring the speed range. You can link to information about how the given value has been determined using the valueReference property.</summary>
		public QuantitativeValue Speed {get; set;}
		///<summary>The position of the steering wheel or similar device (mostly for cars).</summary>
		public SteeringPositionValue SteeringPosition {get; set;}
		///<summary>The permitted vertical load (TWR) of a trailer attached to the vehicle. Also referred to as Tongue Load Rating (TLR) or Vertical Load Rating (VLR)Typical unit code(s): KGM for kilogram, LBR for poundNote 1: You can indicate additional information in the name of the QuantitativeValue node.Note 2: You may also link to a QualitativeValue node that provides additional information using valueReference.Note 3: Note that you can use minValue and maxValue to indicate ranges.</summary>
		public QuantitativeValue TongueWeight {get; set;}
		///<summary>The permitted weight of a trailer attached to the vehicle.Typical unit code(s): KGM for kilogram, LBR for pound* Note 1: You can indicate additional information in the name of the QuantitativeValue node.* Note 2: You may also link to a QualitativeValue node that provides additional information using valueReference.* Note 3: Note that you can use minValue and maxValue to indicate ranges.</summary>
		public QuantitativeValue TrailerWeight {get; set;}
		///<summary>A short text indicating the configuration of the vehicle, e.g. '5dr hatchback ST 2.5 MT 225 hp' or 'limited edition'.</summary>
		public Text VehicleConfiguration {get; set;}
		///<summary>Information about the engine or engines of the vehicle.</summary>
		public EngineSpecification VehicleEngine {get; set;}
		///<summary>The Vehicle Identification Number (VIN) is a unique serial number used by the automotive industry to identify individual motor vehicles.</summary>
		public Text VehicleIdentificationNumber {get; set;}
		///<summary>The color or color combination of the interior of the vehicle.</summary>
		public Text VehicleInteriorColor {get; set;}
		///<summary>The type or material of the interior of the vehicle (e.g. synthetic fabric, leather, wood, etc.). While most interior types are characterized by the material used, an interior type can also be based on vehicle usage or target audience.</summary>
		public Text VehicleInteriorType {get; set;}
		///<summary>The release date of a vehicle model (often used to differentiate versions of the same make and model).</summary>
		public Bam.Net.Schema.Org.DataTypes.Date VehicleModelDate {get; set;}
		///<summary>The number of passengers that can be seated in the vehicle, both in terms of the physical space available, and in terms of limitations set by law.Typical unit code(s): C62 for persons.</summary>
		public OneOfThese<Number,QuantitativeValue> VehicleSeatingCapacity {get; set;}
		///<summary>Indicates whether the vehicle has been used for special purposes, like commercial rental, driving school, or as a taxi. The legislation in many countries requires this information to be revealed when offering a car for sale.</summary>
		public OneOfThese<CarUsageType,Text> VehicleSpecialUsage {get; set;}
		///<summary>The type of component used for transmitting the power from a rotating power source to the wheels or other relevant component(s) ("gearbox" for cars).</summary>
		public OneOfThese<QualitativeValue,Text,Url> VehicleTransmission {get; set;}
		///<summary>The permitted total weight of the loaded vehicle, including passengers and cargo and the weight of the empty vehicle.Typical unit code(s): KGM for kilogram, LBR for poundNote 1: You can indicate additional information in the name of the QuantitativeValue node.Note 2: You may also link to a QualitativeValue node that provides additional information using valueReference.Note 3: Note that you can use minValue and maxValue to indicate ranges.</summary>
		public QuantitativeValue WeightTotal {get; set;}
		///<summary>The distance between the centers of the front and rear wheels.Typical unit code(s): CMT for centimeters, MTR for meters, INH for inches, FOT for foot/feet</summary>
		public QuantitativeValue Wheelbase {get; set;}
	}
}
