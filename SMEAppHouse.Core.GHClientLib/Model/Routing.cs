/* 
 * GraphHopper Directions API
 *
 * You use the GraphHopper Directions API to add route planning, navigation and route optimization to your software. E.g. the Routing API has turn instructions and elevation data and the Route Optimization API solves your logistic problems and supports various constraints like time window and capacity restrictions. Also it is possible to get all distances between all locations with our fast Matrix API.
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SMEAppHouse.Core.GHClientLib.Model
{
    /// <summary>
    /// Routing
    /// </summary>
    [DataContract]
    public partial class Routing : IEquatable<Routing>, IValidatableObject
    {
        /// <summary>
        /// specifies the data provider
        /// </summary>
        /// <value>specifies the data provider</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum NetworkDataProviderEnum
        {

            /// <summary>
            /// Enum Openstreetmap for value: openstreetmap
            /// </summary>
            [EnumMember(Value = "openstreetmap")]
            Openstreetmap = 1,

            /// <summary>
            /// Enum Tomtom for value: tomtom
            /// </summary>
            [EnumMember(Value = "tomtom")]
            Tomtom = 2
        }

        /// <summary>
        /// specifies the data provider
        /// </summary>
        /// <value>specifies the data provider</value>
        [DataMember(Name = "network_data_provider", EmitDefaultValue = false)]
        public NetworkDataProviderEnum? NetworkDataProvider { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Routing" /> class.
        /// </summary>
        /// <param name="calcPoints">indicates whether solution should come with route geometries.</param>
        /// <param name="considerTraffic">indicates whether historical traffic information should be considered.</param>
        /// <param name="networkDataProvider">specifies the data provider.</param>
        /// <param name="failFast">indicates whether matrix calculation should fail fast when points cannot be connected.</param>
        public Routing(bool? calcPoints = default(bool?), bool? considerTraffic = default(bool?), NetworkDataProviderEnum? networkDataProvider = default(NetworkDataProviderEnum?), bool? failFast = default(bool?))
        {
            this.CalcPoints = calcPoints;
            this.ConsiderTraffic = considerTraffic;
            this.NetworkDataProvider = networkDataProvider;
            this.FailFast = failFast;
        }

        /// <summary>
        /// indicates whether solution should come with route geometries
        /// </summary>
        /// <value>indicates whether solution should come with route geometries</value>
        [DataMember(Name = "calc_points", EmitDefaultValue = false)]
        public bool? CalcPoints { get; set; }

        /// <summary>
        /// indicates whether historical traffic information should be considered
        /// </summary>
        /// <value>indicates whether historical traffic information should be considered</value>
        [DataMember(Name = "consider_traffic", EmitDefaultValue = false)]
        public bool? ConsiderTraffic { get; set; }


        /// <summary>
        /// indicates whether matrix calculation should fail fast when points cannot be connected
        /// </summary>
        /// <value>indicates whether matrix calculation should fail fast when points cannot be connected</value>
        [DataMember(Name = "fail_fast", EmitDefaultValue = false)]
        public bool? FailFast { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Routing {\n");
            sb.Append("  CalcPoints: ").Append(CalcPoints).Append("\n");
            sb.Append("  ConsiderTraffic: ").Append(ConsiderTraffic).Append("\n");
            sb.Append("  NetworkDataProvider: ").Append(NetworkDataProvider).Append("\n");
            sb.Append("  FailFast: ").Append(FailFast).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return Equals(input as Routing);
        }

        /// <summary>
        /// Returns true if Routing instances are equal
        /// </summary>
        /// <param name="input">Instance of Routing to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Routing input)
        {
            if (input == null)
                return false;

            return
                (
                    CalcPoints == input.CalcPoints ||
                    (CalcPoints != null &&
                    CalcPoints.Equals(input.CalcPoints))
                ) &&
                (
                    ConsiderTraffic == input.ConsiderTraffic ||
                    (ConsiderTraffic != null &&
                    ConsiderTraffic.Equals(input.ConsiderTraffic))
                ) &&
                (
                    NetworkDataProvider == input.NetworkDataProvider ||
                    (NetworkDataProvider != null &&
                    NetworkDataProvider.Equals(input.NetworkDataProvider))
                ) &&
                (
                    FailFast == input.FailFast ||
                    (FailFast != null &&
                    FailFast.Equals(input.FailFast))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (CalcPoints != null)
                    hashCode = hashCode * 59 + CalcPoints.GetHashCode();
                if (ConsiderTraffic != null)
                    hashCode = hashCode * 59 + ConsiderTraffic.GetHashCode();
                if (NetworkDataProvider != null)
                    hashCode = hashCode * 59 + NetworkDataProvider.GetHashCode();
                if (FailFast != null)
                    hashCode = hashCode * 59 + FailFast.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
