using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Detrack.DetrackCore
{
    /// <summary>Class where Item is kept</summary>
    [CLSCompliant(true)]
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Item
    {
        private string _sku;
        private string _description;
        private string _purchase_order_number;
        private string _batch_number;
        private string _expiry_date;
        private string _comments;
        private int? _quantity;
        private string _unit_of_measure;
        private bool _checked;
        private int? _actual_quantity;
        private int? _inbound_quantity;
        private string _unload_time_estimate;
        private string _unload_time_actual;
        private int? _follow_up_quantity;
        private string _follow_up_reason;
        private int? _rework_quantity;
        private string _rework_reason;
        private int? _reject_quantity;
        private string _reject_reason;
        private float? _weight;
        private string[] _serial_numbers;

        public string SKU
        {
            get
            {
                return _sku;
            }
            set
            {
                _sku = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public string PurchaseOrderNumber
        {
            get
            {
                return _purchase_order_number;
            }
            set
            {
                _purchase_order_number = value;
            }
        }

        public string BatchNumber
        {
            get
            {
                return _batch_number;
            }
            set
            {
                _batch_number = value;
            }
        }

        public string ExpiryDate
        {
            get
            {
                return _expiry_date;
            }
            set
            {
                _expiry_date = value;
            }
        }

        public string Comments
        {
            get
            {
                return _comments;
            }
            set
            {
                _comments = value;
            }
        }

        public int? Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
            }
        }

        public string UnitOfMeasure
        {
            get
            {
                return _unit_of_measure;
            }
            set
            {
                _unit_of_measure = value;
            }
        }

        public bool Checked
        {
            get
            {
                return _checked;
            }
            set
            {
                _checked = value;
            }
        }

        public int? ActualQuantity
        {
            get
            {
                return _actual_quantity;
            }
            set
            {
                _actual_quantity = value;
            }
        }

        public int? InboundQuantity
        {
            get
            {
                return _inbound_quantity;
            }
            set
            {
                _inbound_quantity = value;
            }
        }

        public string UnloadTimeEstimate
        {
            get
            {
                return _unload_time_estimate;
            }
            set
            {
                _unload_time_estimate = value;
            }
        }

        public string UnloadTimeActual
        {
            get
            {
                return _unload_time_actual;
            }
            set
            {
                _unload_time_actual = value;
            }
        }

        public int? FollowUpQuantity
        {
            get
            {
                return _follow_up_quantity;
            }
            set
            {
                _follow_up_quantity = value;
            }
        }

        public string FollowUpReason
        {
            get
            {
                return _follow_up_reason;
            }
            set
            {
                _follow_up_reason = value;
            }
        }

        public int? ReworkQuantity
        {
            get
            {
                return _rework_quantity;
            }
            set
            {
                _rework_quantity = value;
            }
        }

        public string ReworkReason
        {
            get
            {
                return _rework_reason;
            }
            set
            {
                _rework_reason = value;
            }
        }

        public int? RejectQuantity
        {
            get
            {
                return _reject_quantity;
            }
            set
            {
                _reject_quantity = value;
            }
        }

        public string RejectReason
        {
            get
            {
                return _reject_reason;
            }
            set
            {
                _reject_reason = value;
            }
        }

        public float? Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
            }
        }

        public string[] SerialNumbers
        {
            get
            {
                return _serial_numbers;
            }
            set
            {
                _serial_numbers = value;
            }
        }

        public string ID { get; }

        public string PhotoURL { get; }
    }
}

