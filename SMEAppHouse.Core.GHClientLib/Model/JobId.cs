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

namespace SMEAppHouse.Core.GHClientLib.Model
{
    /// <summary>
    /// JobId
    /// </summary>
    [DataContract]
    public partial class JobId :  IEquatable<JobId>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobId" /> class.
        /// </summary>
        /// <param name="_JobId">unique id for your job/request with which you can fetch your solution.</param>
        public JobId(string _JobId = default(string))
        {
            this._JobId = _JobId;
        }
        
        /// <summary>
        /// unique id for your job/request with which you can fetch your solution
        /// </summary>
        /// <value>unique id for your job/request with which you can fetch your solution</value>
        [DataMember(Name="job_id", EmitDefaultValue=false)]
        public string _JobId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class JobId {\n");
            sb.Append("  _JobId: ").Append(_JobId).Append("\n");
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
            return Equals(input as JobId);
        }

        /// <summary>
        /// Returns true if JobId instances are equal
        /// </summary>
        /// <param name="input">Instance of JobId to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(JobId input)
        {
            if (input == null)
                return false;

            return 
                (
                    _JobId == input._JobId ||
                    (_JobId != null &&
                    _JobId.Equals(input._JobId))
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
                if (_JobId != null)
                    hashCode = hashCode * 59 + _JobId.GetHashCode();
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
