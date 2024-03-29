﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

[assembly: CLSCompliant(true)]
namespace Detrack.DetrackCore
{
    /// <summary>
    /// Status can only be chosen from this enum.
    /// </summary>
    [CLSCompliant(true)]
    public enum Status
    {
        /// <summary>Job have been received/></summary>
        info_recv,

        /// <summary>Job is out for delivery</summary>
        dispatched,

        /// <summary>Job have been completed</summary>
        completed,

        /// <summary>Job is only partially completed</summary>
        completed_partial,

        /// <summary>Job has failed just ike how life has failed you</summary>
        failed
    }

    /// <summary>
    /// Contains all properties and methods for communicating with the API
    /// </summary>
    [CLSCompliant(true)]
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Job : INotifyPropertyChanged
    {
        #region
        private static HttpClient client = new HttpClient();
        [JsonIgnore]
        private HashSet<string> _modified_properties = new HashSet<string>();
        private string _do_number;
        private string _address;
        private string _date;
        private string _assign_to;
        private Status? _status;
        private string _type;
        private string _primary_job_status;
        private bool _open_to_marketplace;
        private float? _marketplace_offer;
        private string _start_date;
        private string _job_release_time;
        private string _job_time;
        private string _time_window;
        private string _job_received_date;
        private string _tracking_number;
        private string _order_number;
        private string _job_type;
        private int? _job_sequence;
        private string _job_fee;
        private float? _address_lat;
        private float? _address_lng;
        private string _company_name;
        private string _address_1;
        private string _address_2;
        private string _address_3;
        private string _postal_code;
        private string _city;
        private string _state;
        private string _country;
        private string _billing_address;
        private string _deliver_to_collect_from;
        private string _last_name;
        private string _phone_number;
        private string _sender_phone_number;
        private string _fax_number;
        private string _instructions;
        private string _notify_email;
        private string _webhook_url;
        private string _zone;
        private string _customer;
        private string _account_no;
        private string _job_owner;
        private string _invoice_number;
        private float? _invoice_amount;
        private string _payment_mode;
        private float? _payment_amount;
        private string _group_id; // datatype unknown update later
        private string _group_name;
        private string _vendor_name;
        private string _source;
        private float? _weight;
        private float? _parcel_width;
        private float? _parcel_length;
        private float? _parcel_height;
        private float? _cubic_meter;
        private string _boxes;
        private int? _cartons;
        private int? _pieces;
        private int? _envelopes;
        private int? _pallets;
        private int? _bins;
        private int? _trays;
        private int? _bundles;
        private int? _rolls;
        private int _number_of_shipping_labels;
        private int? _attachment_url;
        private string _carrier;
        private bool? _auto_reschedule;
        private string _eta_time;
        private string _depot;
        private string _depot_contact;
        private string _department;
        private string _sales_person;
        private string _identification_number;
        private string _bank_prefix;
        private string _run_number;
        private string _pick_up_from;
        private string _pick_up_time;
        private string _pick_up_lat;
        private string _pick_up_lng;
        private string _pick_up_address;
        private string _pick_up_address_1;
        private string _pick_up_address_2;
        private string _pick_up_address_3;
        private string _pick_up_city;
        private string _pick_up_state;
        private string _pick_up_country;
        private string _pick_up_postal_code;
        private string _pick_up_zone;
        private float? _job_price;
        private float? _insurance_price;
        private bool _insurance_coverage;
        private float? _total_price;
        private string _payer_type;
        private string _remarks;
        private string _service_type;
        private string _warehouse_address;
        private string _destination_time_window;
        private string _door;
        private string _time_zone;
        private List<Item> _items;
        private string _pod_time;

        /// <summary>
        /// Initializes a new instance of the <see cref="Job"/> class.
        /// </summary>
        /// <param name="date">Used to indicate date in which the job is made.</param>
        /// <param name="address">Used to indicate the address of the job.</param>
        /// <param name="doNumber">The given DO Number for the job.</param>
        public Job(string doNumber, string date, string address)
        {
            if (date != string.Empty &&
                address != string.Empty &&
                doNumber != string.Empty)
            {
                Address = address;
                DONumber = doNumber;
                Date = date;
                Items = new List<Item>();
            }
            else
            {
                throw new DONumberEmptyException("DO Number, Address or Date cannot be empty!");
            }
        }

        public static string DefaultApiKey { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string DONumber
        {
            get
            {
                return _do_number;
            }
            set
            {
                if (_do_number != value)
                {
                    OnPropertyChanged();
                }
                _do_number = value;
            }
        }

        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                if (_address != value)
                {
                    OnPropertyChanged();
                }
                _address = value;
            }
        }

        public string Date
        {
            get
            {
                return _date;
            }
            set
            {
                if (DateChecker(value))
                {
                    if (_date != value)
                    {
                        OnPropertyChanged();
                    }
                    _date = value;
                }
            }
        }

        public string AssignTo
        {
            get
            {
                return _assign_to;
            }
            set
            {
                if (_assign_to != value)
                {
                    OnPropertyChanged();
                }
                _assign_to = value;
            }
        }

        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (_type != value)
                {
                    OnPropertyChanged();
                }
                _type = value;
            }
        }

        public string PrimaryJobStatus
        {
            get
            {
                return _primary_job_status;
            }
            set
            {
                if (_primary_job_status != value)
                {
                    OnPropertyChanged();
                }
                _primary_job_status = value;
            }
        }

        public bool OpenToMarketplace
        {
            get
            {
                return _open_to_marketplace;
            }
            set
            {
                if (_open_to_marketplace != value)
                {
                    OnPropertyChanged();
                }
                _open_to_marketplace = value;
            }
        }

        public float? MarketplaceOffer
        {
            get
            {
                return _marketplace_offer;
            }
            set
            {
                if (_marketplace_offer != value)
                {
                    OnPropertyChanged();
                }
                _marketplace_offer = value;
            }
        }

        public string StartDate
        {
            get
            {
                return _start_date;
            }
            set
            {
                if (_start_date != value)
                {
                    OnPropertyChanged();
                }
                _start_date = value;
            }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public Status? Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    OnPropertyChanged();
                }
                _status = value;
            }
        }

        public string JobReleaseTime
        {
            get
            {
                return _job_release_time;
            }
            set
            {
                if (_job_release_time != value)
                {
                    OnPropertyChanged();
                }
                _job_release_time = value;
            }
        }

        public string JobTime
        {
            get
            {
                return _job_time;
            }
            set
            {
                if (_job_time != value)
                {
                    OnPropertyChanged();
                }
                _job_time = value;
            }
        }

        public string TimeWindow
        {
            get
            {
                return _time_window;
            }
            set
            {
                if (_time_window != value)
                {
                    OnPropertyChanged();
                }
                _time_window = value;
            }
        }

        public string JobReceivedDate
        {
            get
            {
                return _job_received_date;
            }
            set
            {
                if (_job_received_date != value)
                {
                    OnPropertyChanged();
                }
                _job_received_date = value;
            }
        }

        public string TrackingNumber
        {
            get
            {
                return _tracking_number;
            }
            set
            {
                if (_tracking_number != value)
                {
                    OnPropertyChanged();
                }
                _tracking_number = value;
            }
        }

        public string OrderNumber
        {
            get
            {
                return _order_number;
            }
            set
            {
                if (_order_number != value)
                {
                    OnPropertyChanged();
                }
                _order_number = value;
            }
        }

        public string JobType
        {
            get
            {
                return _job_type;
            }
            set
            {
                if (_job_type != value)
                {
                    OnPropertyChanged();
                }
                _job_type = value;
            }
        }

        public int? JobSequence
        {
            get
            {
                return _job_sequence;
            }
            set
            {
                if (_job_sequence != value)
                {
                    OnPropertyChanged();
                }
                _job_sequence = value;
            }
        }

        public string JobFee
        {
            get
            {
                return _job_fee;
            }
            set
            {
                if (_job_fee != value)
                {
                    OnPropertyChanged();
                }
                _job_fee = value;
            }
        }

        public float? AddressLat
        {
            get
            {
                return _address_lat;
            }
            set
            {
                if (_address_lat != value)
                {
                    OnPropertyChanged();
                }
                _address_lat = value;
            }
        }

        public float? AddressLng
        {
            get
            {
                return _address_lng;
            }
            set
            {
                if (_address_lng != value)
                {
                    OnPropertyChanged();
                }
                _address_lng = value;
            }
        }

        public string CompanyName
        {
            get
            {
                return _company_name;
            }
            set
            {
                if (_company_name != value)
                {
                    OnPropertyChanged();
                }
                _company_name = value;
            }
        }

        public string Address1
        {
            get
            {
                return _address_1;
            }
            set
            {
                if (_address_1 != value)
                {
                    OnPropertyChanged();
                }
                _address_1 = value;
            }
        }

        public string Address2
        {
            get
            {
                return _address_2;
            }
            set
            {
                if (_address_2 != value)
                {
                    OnPropertyChanged();
                }
                _address_2 = value;
            }
        }

        public string Address3
        {
            get
            {
                return _address_3;
            }
            set
            {
                if (_address_3 != value)
                {
                    OnPropertyChanged();
                }
                _address_3 = value;
            }
        }

        public string PostalCode
        {
            get
            {
                return _postal_code;
            }
            set
            {
                if (_postal_code != value)
                {
                    OnPropertyChanged();
                }
                _postal_code = value;
            }
        }

        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                if (_city != value)
                {
                    OnPropertyChanged();
                }
                _city = value;
            }
        }

        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state != value)
                {
                    OnPropertyChanged();
                }
                _state = value;
            }
        }

        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                if (_country != value)
                {
                    OnPropertyChanged();
                }
                _country = value;
            }
        }

        public string BillingAddress
        {
            get
            {
                return _billing_address;
            }
            set
            {
                if (_billing_address != value)
                {
                    OnPropertyChanged();
                }
                _billing_address = value;
            }
        }

        public string DeliverToCollectFrom
        {
            get
            {
                return _deliver_to_collect_from;
            }
            set
            {
                if (_deliver_to_collect_from != value)
                {
                    OnPropertyChanged();
                }
                _deliver_to_collect_from = value;
            }
        }

        public string LastName
        {
            get
            {
                return _last_name;
            }
            set
            {
                if (_last_name != value)
                {
                    OnPropertyChanged();
                }
                _last_name = value;
            }
        }

        public string PhoneNumber
        {
            get
            {
                return _phone_number;
            }
            set
            {
                if (_phone_number != value)
                {
                    OnPropertyChanged();
                }
                _phone_number = value;
            }
        }

        public string SenderPhoneNumber
        {
            get
            {
                return _sender_phone_number;
            }
            set
            {
                if (_sender_phone_number != value)
                {
                    OnPropertyChanged();
                }
                _sender_phone_number = value;
            }
        }

        public string FaxNumber
        {
            get
            {
                return _fax_number;
            }
            set
            {
                if (_fax_number != value)
                {
                    OnPropertyChanged();
                }
                _fax_number = value;
            }
        }

        public string Instructions
        {
            get
            {
                return _instructions;
            }
            set
            {
                if (_instructions != value)
                {
                    OnPropertyChanged();
                }
                _instructions = value;
            }
        }

        public string NotifyEmail
        {
            get
            {
                return _notify_email;
            }
            set
            {
                if (_notify_email != value)
                {
                    OnPropertyChanged();
                }
                _notify_email = value;
            }
        }

        public string WebhookUrl
        {
            get
            {
                return _webhook_url;
            }
            set
            {
                if (_webhook_url != value)
                {
                    OnPropertyChanged();
                }
                _webhook_url = value;
            }
        }

        public string Zone
        {
            get
            {
                return _zone;
            }
            set
            {
                if (_zone != value)
                {
                    OnPropertyChanged();
                }
                _zone = value;
            }
        }

        public string Customer
        {
            get
            {
                return _customer;
            }
            set
            {
                if (_customer != value)
                {
                    OnPropertyChanged();
                }
                _customer = value;
            }
        }

        public string AccountNo
        {
            get
            {
                return _account_no;
            }
            set
            {
                if (_account_no != value)
                {
                    OnPropertyChanged();
                }
                _account_no = value;
            }
        }

        public string JobOwner
        {
            get
            {
                return _job_owner;
            }
            set
            {
                if (_job_owner != value)
                {
                    OnPropertyChanged();
                }
                _job_owner = value;
            }
        }

        public string InvoiceNumber
        {
            get
            {
                return _invoice_number;
            }
            set
            {
                if (_invoice_number != value)
                {
                    OnPropertyChanged();
                }
                _invoice_number = value;
            }
        }

        public float? InvoiceAmount
        {
            get
            {
                return _invoice_amount;
            }
            set
            {
                if (_invoice_amount != value)
                {
                    OnPropertyChanged();
                }
                _invoice_amount = value;
            }
        }

        public string PaymentMode
        {
            get
            {
                return _payment_mode;
            }
            set
            {
                if (_payment_mode != value)
                {
                    OnPropertyChanged();
                }
                _payment_mode = value;
            }
        }

        public float? PaymentAmount
        {
            get
            {
                return _payment_amount;
            }
            set
            {
                if (_payment_amount != value)
                {
                    OnPropertyChanged();
                }
                _payment_amount = value;
            }
        }

        public string GroupID
        {
            get
            {
                return _group_id;
            }
            set
            {
                if (_group_id != value)
                {
                    OnPropertyChanged();
                }
                _group_id = value;
            }
        }

        public string GroupName
        {
            get
            {
                return _group_name;
            }
            set
            {
                if (_group_name != value)
                {
                    OnPropertyChanged();
                }
                _group_name = value;
            }
        }

        public string VendorName
        {
            get
            {
                return _vendor_name;
            }
            set
            {
                if (_vendor_name != value)
                {
                    OnPropertyChanged();
                }
                _vendor_name = value;
            }
        }

        public string Source
        {
            get
            {
                return _source;
            }
            set
            {
                if (_source != value)
                {
                    OnPropertyChanged();
                }
                _source = value;
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
                if (_weight != value)
                {
                    OnPropertyChanged();
                }
                _weight = value;
            }
        }

        public float? ParcelWidth
        {
            get
            {
                return _parcel_width;
            }
            set
            {
                if (_parcel_width != value)
                {
                    OnPropertyChanged();
                }
                _parcel_width = value;
            }
        }

        public float? ParcelLength
        {
            get
            {
                return _parcel_length;
            }
            set
            {
                if (_parcel_length != value)
                {
                    OnPropertyChanged();
                }
                _parcel_length = value;
            }
        }

        public float? ParcelHeight
        {
            get
            {
                return _parcel_height;
            }
            set
            {
                if (_parcel_height != value)
                {
                    OnPropertyChanged();
                }
                _parcel_height = value;
            }
        }

        public float? CubicMeter
        {
            get
            {
                return _cubic_meter;
            }
            set
            {
                if (_cubic_meter != value)
                {
                    OnPropertyChanged();
                }
                _cubic_meter = value;
            }
        }

        public string Boxes
        {
            get
            {
                return _boxes;
            }
            set
            {
                if (_boxes != value)
                {
                    OnPropertyChanged();
                }
                _boxes = value;
            }
        }

        public int? Cartons
        {
            get
            {
                return _cartons;
            }
            set
            {
                if (_cartons != value)
                {
                    OnPropertyChanged();
                }
                _cartons = value;
            }
        }

        public int? Pieces
        {
            get
            {
                return _pieces;
            }
            set
            {
                if (_pieces != value)
                {
                    OnPropertyChanged();
                }
                _pieces = value;
            }
        }

        public int? Envelopes
        {
            get
            {
                return _envelopes;
            }
            set
            {
                if (_envelopes != value)
                {
                    OnPropertyChanged();
                }
                _envelopes = value;
            }
        }

        public int? Pallets
        {
            get
            {
                return _pallets;
            }
            set
            {
                if (_pallets != value)
                {
                    OnPropertyChanged();
                }
                _pallets = value;
            }
        }

        public int? Bins
        {
            get
            {
                return _bins;
            }
            set
            {
                if (_bins != value)
                {
                    OnPropertyChanged();
                }
                _bins = value;
            }
        }

        public int? Trays
        {
            get
            {
                return _trays;
            }
            set
            {
                if (_trays != value)
                {
                    OnPropertyChanged();
                }
                _trays = value;
            }
        }

        public int? Bundles
        {
            get
            {
                return _bundles;
            }
            set
            {
                if (_bundles != value)
                {
                    OnPropertyChanged();
                }
                _bundles = value;
            }
        }

        public int? Rolls
        {
            get
            {
                return _rolls;
            }
            set
            {
                if (_rolls != value)
                {
                    OnPropertyChanged();
                }
                _rolls = value;
            }
        }

        public int NumberOfShippingLables
        {
            get
            {
                return _number_of_shipping_labels;
            }
            set
            {
                if (_number_of_shipping_labels != value)
                {
                    OnPropertyChanged();
                }
                _number_of_shipping_labels = value;
            }
        }

        public int? AttachmentUrl
        {
            get
            {
                return _attachment_url;
            }
            set
            {
                if (_attachment_url != value)
                {
                    OnPropertyChanged();
                }
                _attachment_url = value;
            }
        }

        public string Carrier
        {
            get
            {
                return _carrier;
            }
            set
            {
                if (_carrier != value)
                {
                    OnPropertyChanged();
                }
                _carrier = value;
            }
        }

        public bool? AutoReschedule
        {
            get
            {
                return _auto_reschedule;
            }
            set
            {
                if (_auto_reschedule != value)
                {
                    OnPropertyChanged();
                }
                _auto_reschedule = value;
            }
        }

        public string ETATime
        {
            get
            {
                return _eta_time;
            }
            set
            {
                if (_eta_time != value)
                {
                    OnPropertyChanged();
                }
                _eta_time = value;
            }
        }

        public string Depot
        {
            get
            {
                return _depot;
            }
            set
            {
                if (_depot != value)
                {
                    OnPropertyChanged();
                }
                _depot = value;
            }
        }

        public string DepotContact
        {
            get
            {
                return _depot_contact;
            }
            set
            {
                if (_depot_contact != value)
                {
                    OnPropertyChanged();
                }
                _depot_contact = value;
            }
        }

        public string Department
        {
            get
            {
                return _department;
            }
            set
            {
                if (_department != value)
                {
                    OnPropertyChanged();
                }
                _department = value;
            }
        }

        public string SalesPerson
        {
            get
            {
                return _sales_person;
            }
            set
            {
                if (_sales_person != value)
                {
                    OnPropertyChanged();
                }
                _sales_person = value;
            }
        }

        public string IdentificationNumber
        {
            get
            {
                return _identification_number;
            }
            set
            {
                if (_identification_number != value)
                {
                    OnPropertyChanged();
                }
                _identification_number = value;
            }
        }

        public string BankPrefix
        {
            get
            {
                return _bank_prefix;
            }
            set
            {
                if (_bank_prefix != value)
                {
                    OnPropertyChanged();
                }
                _bank_prefix = value;
            }
        }

        public string RunNumber
        {
            get
            {
                return _run_number;
            }
            set
            {
                if (_run_number != value)
                {
                    OnPropertyChanged();
                }
                _run_number = value;
            }
        }

        public string PickUpFrom
        {
            get
            {
                return _pick_up_from;
            }
            set
            {
                if (_pick_up_from != value)
                {
                    OnPropertyChanged();
                }
                _pick_up_from = value;
            }
        }

        public string PickUpTime
        {
            get
            {
                return _pick_up_time;
            }
            set
            {
                if (_pick_up_time != value)
                {
                    OnPropertyChanged();
                }
                _pick_up_time = value;
            }
        }

        public string PickUpLat
        {
            get
            {
                return _pick_up_lat;
            }
            set
            {
                if (_pick_up_lat != value)
                {
                    OnPropertyChanged();
                }
                _pick_up_lat = value;
            }
        }

        public string PickUpLng
        {
            get
            {
                return _pick_up_lng;
            }
            set
            {
                if (_pick_up_lng != value)
                {
                    OnPropertyChanged();
                }
                _pick_up_lng = value;
            }
        }

        public string PickUpAddress
        {
            get
            {
                return _pick_up_address;
            }
            set
            {
                if (_pick_up_address != value)
                {
                    OnPropertyChanged();
                }
                _pick_up_address = value;
            }
        }

        public string PickUpAddress1
        {
            get
            {
                return _pick_up_address_1;
            }
            set
            {
                if (_pick_up_address_1 != value)
                {
                    OnPropertyChanged();
                }
                _pick_up_address_1 = value;
            }
        }

        public string PickUpAddress2
        {
            get
            {
                return _pick_up_address_2;
            }
            set
            {
                if (_pick_up_address_2 != value)
                {
                    OnPropertyChanged();
                }
                _pick_up_address_2 = value;
            }
        }

        public string PickUpAddress3
        {
            get
            {
                return _pick_up_address_3;
            }
            set
            {
                if (_pick_up_address_3 != value)
                {
                    OnPropertyChanged();
                }
                _pick_up_address_3 = value;
            }
        }

        public string PickUpCity
        {
            get
            {
                return _pick_up_city;
            }
            set
            {
                if (_pick_up_city != value)
                {
                    OnPropertyChanged();
                }
                _pick_up_city = value;
            }
        }

        public string PickUpState
        {
            get
            {
                return _pick_up_state;
            }
            set
            {
                if (_pick_up_state != value)
                {
                    OnPropertyChanged();
                }
                _pick_up_state = value;
            }
        }

        public string PickUpCountry
        {
            get
            {
                return _pick_up_country;
            }
            set
            {
                if (_pick_up_country != value)
                {
                    OnPropertyChanged();
                }
                _pick_up_country = value;
            }
        }

        public string PickUpPostalCode
        {
            get
            {
                return _pick_up_postal_code;
            }
            set
            {
                if (_pick_up_postal_code != value)
                {
                    OnPropertyChanged();
                }
                _pick_up_postal_code = value;
            }
        }

        public string PickUpZone
        {
            get
            {
                return _pick_up_zone;
            }
            set
            {
                if (_pick_up_zone != value)
                {
                    OnPropertyChanged();
                }
                _pick_up_zone = value;
            }
        }

        public float? JobPrice
        {
            get
            {
                return _job_price;
            }
            set
            {
                if (_job_price != value)
                {
                    OnPropertyChanged();
                }
                _job_price = value;
            }
        }

        public float? InsurancePrice
        {
            get
            {
                return _insurance_price;
            }
            set
            {
                if (_insurance_price != value)
                {
                    OnPropertyChanged();
                }
                _insurance_price = value;
            }
        }

        public bool InsuranceCoverage
        {
            get
            {
                return _insurance_coverage;
            }
            set
            {
                if (_insurance_coverage != value)
                {
                    OnPropertyChanged();
                }
                _insurance_coverage = value;
            }
        }

        public float? TotalPrice
        {
            get
            {
                return _total_price;
            }
            set
            {
                if (_total_price != value)
                {
                    OnPropertyChanged();
                }
                _total_price = value;
            }
        }

        public string PayerType
        {
            get
            {
                return _payer_type;
            }
            set
            {
                if (_payer_type != value)
                {
                    OnPropertyChanged();
                }
                _payer_type = value;
            }
        }

        public string Remarks
        {
            get
            {
                return _remarks;
            }
            set
            {
                if (_remarks != value)
                {
                    OnPropertyChanged();
                }
                _remarks = value;
            }
        }

        public string ServiceType
        {
            get
            {
                return _service_type;
            }
            set
            {
                if (_service_type != value)
                {
                    OnPropertyChanged();
                }
                _service_type = value;
            }
        }

        public string WarehouseAddress
        {
            get
            {
                return _warehouse_address;
            }
            set
            {
                if (_warehouse_address != value)
                {
                    OnPropertyChanged();
                }
                _warehouse_address = value;
            }
        }

        public string DestinationTimeWindow
        {
            get
            {
                return _destination_time_window;
            }
            set
            {
                if (_destination_time_window != value)
                {
                    OnPropertyChanged();
                }
                _destination_time_window = value;
            }
        }

        public string Door
        {
            get
            {
                return _door;
            }
            set
            {
                if (_door != value)
                {
                    OnPropertyChanged();
                }
                _door = value;
            }
        }

        public string TimeZone
        {
            get
            {
                return _time_zone;
            }
            set
            {
                if (_time_zone != value)
                {
                    OnPropertyChanged();
                }
                _time_zone = value;
            }
        }

        public List<Item> Items
        {
            get
            {
                return _items;
            }
            set
            {
                if (_items != value)
                {
                    OnPropertyChanged();
                }
                _items = value;
            }
        }

        public string PODTime
        {
            get
            {
                return _pod_time;
            }
            set
            {
                if (_pod_time != value)
                {
                    OnPropertyChanged();
                }
                _pod_time = value;
            }
        }
        #endregion
        /// <summary>
        /// Retrieves a job from the database. This is a <see langword="static" /> method.
        /// This function's call must be awaited.
        /// </summary>
        /// <returns>A Json string containing info of the retrieved job</returns>
        /// <param name="doNumber">The DO Number of the job to retrieve</param>
        /// <param name="date">The date of the job to retrieve. If <see langword="null" />,
        /// it will take the job with the latest date</param>
        public static async Task<Job> RetrieveJob(string doNumber, string date = null)
        {
            IsAPIEmpty(DefaultApiKey);
            doNumber = HttpUtility.UrlEncode(doNumber);
            string link = $"dn/jobs/{doNumber}";
            if (date != null)
            {
                DateChecker(date);
                link += $"/{date}";
            }

            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(string.Format("https://app.detrack.com/api/v2/{0}", link)),
                Method = HttpMethod.Get,
                Headers =
                {
                    {
                        "X-API-KEY", DefaultApiKey
                    },
                    {
                        HttpRequestHeader.ContentType.ToString(), "application/json"
                    }
                }
            };

            HttpResponseMessage response = await client.SendAsync(request);
            string responseData = await response.Content.ReadAsStringAsync();
            CheckForExceptions(responseData);
            string data = JObject.Parse(responseData)["data"].ToString();
            Job responseObject = JsonConvert.DeserializeObject<Job>(data);
            return responseObject;
        }

        /// <summary>in
        /// Delete a job from the database. This is a <see langword="static" /> method.
        /// This function's call must be awaited.
        /// </summary>
        /// <param name="doNumber">The DO Number of the job to delete</param>
        /// <param name="date">The date of the job to delete. If <see langword="null" />,
        /// it will take the job with the latest date</param>
        public static async Task DeleteJob(string doNumber, string date = null)
        {
            IsAPIEmpty(DefaultApiKey);
            doNumber = HttpUtility.UrlEncode(doNumber);
            string link = $"dn/jobs/{doNumber}";
            if (date != null)
            {
                DateChecker(date);
                link += $"/{date}";
            }

            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://app.detrack.com/api/v2/{link}"),
                Method = HttpMethod.Delete,
                Headers =
                {
                    {
                        "X-API-KEY", DefaultApiKey
                    },
                    {
                        HttpRequestHeader.ContentType.ToString(), "application/json"
                    }
                }
            };

            HttpResponseMessage response = await client.SendAsync(request);
            string responseData = await response.Content.ReadAsStringAsync();
            CheckForExceptions(responseData);
        }

        /// <summary>
        /// Gets all the job from the database based on the given parameter.
        /// </summary>
        /// <returns>A JSON string containing all the jobs' information</returns>
        /// <param name="paramList">List of parameters(page, limit, date, type,
        /// assignTo, Status, DONumber) and its value.</param>
        public static async Task<List<Job>> ListAllJobs(Dictionary<string, string> paramList)
        {
            IsAPIEmpty(DefaultApiKey);
            StringBuilder link = new StringBuilder();
            List<string> parameter = new List<string>
            {
                "page",
                "limit",
                "date",
                "type",
                "assignTo",
                "status",
                "DONumber"
            };
            foreach (string key in paramList.Keys)
            {
                if (parameter.Contains(key) is false)
                {
                    throw new InvalidParameterException($"{key} is an invalid key");
                }
                    
                switch (key)
                {
                    case "assignTo":
                        if (paramList["assignTo"] == "unassigned")
                        {
                            link.Append("&assign_to");
                        }
                        if (paramList["assignTo"] != null)
                        {
                            link.Append($"&assign_to={paramList["assignTo"]}");
                        }
                        break;
                    case "date":
                        DateChecker(paramList["date"]);
                        link.Append($"&date={paramList["date"]}");
                        break;
                    case "DONumber":
                        link.Append($"&do_number={paramList["DONumber"]}");
                        break;
                    case "status":
                        link.Append($"&status={paramList["status"]}");
                        break;
                    case "limit":
                        link.Append($"&limit={paramList["limit"]}");
                        break;
                    case "page":
                        link.Append($"&page={paramList["page"]}");
                        break;
                    case "type":
                        link.Append($"&type={paramList["type"]}");
                        break;
                }
            }

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://app.detrack.com/api/v2/dn/jobs?{link}"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {
                        "X-API-KEY", DefaultApiKey
                    },
                    {
                        HttpRequestHeader.ContentType.ToString(), "application/json"
                    }
                }
            };

            HttpResponseMessage response = await client.SendAsync(request);
            string responseData = await response.Content.ReadAsStringAsync();
            CheckForExceptions(responseData);
            JToken data = JObject.Parse(responseData)["data"];
            List<Job> joblist = new List<Job>();

            foreach (var item in data)
            {
                Job job = JsonConvert.DeserializeObject<Job>(item.ToString());
                joblist.Add(job);
            }
            return joblist;
        }

        /// <summary>
        /// Reattempts a failed job. Will throw error if used to reattempt a
        /// completed or partially completed job.
        /// </summary>
        /// <param name="doNumber">The DO Number of the job to reattempt.</param>
        /// <param name="date">The date of the job to reattempt. If <see langword="null" />,
        /// it will take the job with the latest date</param>
        public static async Task ReattemptJob(string doNumber, string date)
        {
            IsAPIEmpty(DefaultApiKey);
            DateChecker(date);
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://app.detrack.com/api/v2/dn/jobs/reattempt"),
                Headers =
                {
                    {
                        "X-API-KEY", DefaultApiKey
                    },
                    {
                        HttpRequestHeader.ContentType.ToString(), "application/json"
                    }
                },
                Content = new StringContent($"{{ \"data\": {{ \"do_number\": \"{doNumber}\", \"date\": \"{date}\" }} }}",
                                            Encoding.Default,
                                            "application/json")
            };

            HttpResponseMessage response = await client.SendAsync(request);
            string responseData = await response.Content.ReadAsStringAsync();
            CheckForExceptions(responseData);
        }

        /// <summary>
        /// dCreates multiple jobs on the database in a single http call.
        /// This is a <see langword="static" /> method. This function's call
        /// must be awaited.
        /// </summary>
        /// <param name="jobList">A list containing instances of <see cref="T:Detrack.DetrackCore.Job" />
        /// to create.</param>
        public static async Task CreateJobs(List<Job> jobList)
        {
            IsAPIEmpty(DefaultApiKey);
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://app.detrack.com/api/v2/dn/jobs/bulk"),
                Method = HttpMethod.Post,
                Headers =
                {
                    {
                        "X-API-KEY", DefaultApiKey
                    },
                    {
                        HttpRequestHeader.ContentType.ToString(), "application/json"
                    }
                },
                Content = new StringContent(ClassToJsonList(jobList),
                                            Encoding.Default,
                                            "application/json")
            };

            HttpResponseMessage response = await client.SendAsync(request);
            string responseData = await response.Content.ReadAsStringAsync();
            CheckForExceptionsInBatches(responseData);
        }

        /// <summary>
        /// Updates multiple jobs on the database in a single Http call.
        /// This is a <see langword="static" /> method. This function's call
        /// must be awaited.
        /// </summary>
        /// <param name="jobList">A list containing instances of <see cref="T:Detrack.DetrackCore.Job" />
        /// to update.</param>
        public static async Task UpdateJobs(List<Job> jobList)
        {
            IsAPIEmpty(DefaultApiKey);
            List<Dictionary<string, object>> dictList = new List<Dictionary<string, object>>();
            foreach (Job job in jobList)
            {
                Dictionary<string, object> updatedDict = new Dictionary<string, object>();
                foreach (string modifiedProperty in job._modified_properties)
                {
                    string item = modifiedProperty;
                    switch (item)
                    {
                        case "Items":
                        {
                            if (job.Items.Count > 0)
                                updatedDict.Add(ToSnake(item), job.Items);
                            break;
                        }
                        case "Status":
                            updatedDict.Add(ToSnake(item), job.Status.ToString());
                            break;
                        default:
                        {
                            PropertyInfo property = job.GetType().GetProperty(item);
                            object value = property?.GetValue(job);
                            updatedDict.Add(ToSnake(item), value);
                            break;
                        }
                    }
                }
                dictList.Add(updatedDict);
            }

            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://app.detrack.com/api/v2/dn/jobs"),
                Method = HttpMethod.Put,
                Headers =
                {
                    {
                        "X-API-KEY", DefaultApiKey
                    },
                    {
                        HttpRequestHeader.ContentType.ToString(), "application/json"
                    }
                },
                Content = new StringContent(ClassToJsonListUpdate(dictList),
                                            Encoding.Default,
                                            "application/json")
            };

            HttpResponseMessage response = await client.SendAsync(request);
            string responseData = await response.Content.ReadAsStringAsync();
            CheckForExceptionsInBatches(responseData);
        }

        /// <summary>
        /// Deletes multiple jobs from the database in a single htpp call.
        /// This is a <see langword="static" /> method. This function's call
        /// must be awaited.
        /// </summary>
        /// <returns>The jobs.</returns>
        /// <param name="jobList">Job list.</param>
        public static async Task DeleteJobs(List<Job> jobList)
        {
            IsAPIEmpty(DefaultApiKey);
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("https://app.detrack.com/api/v2/dn/jobs"),
                Headers =
                {
                    {
                        "X-API-KEY", DefaultApiKey
                    },
                    {
                        HttpRequestHeader.ContentType.ToString(), "application/json"
                    }
                },
                Content = new StringContent(ClassToJsonList(jobList),
                                            Encoding.Default,
                                            "application/json")
            };

            HttpResponseMessage response = await client.SendAsync(request);
            string responseData = await response.Content.ReadAsStringAsync();
            CheckForExceptionsInBatches(responseData);
        }

        /// <summary>Downloads the job export in pdf form.</summary>
        /// <returns>The job export.</returns>
        /// <param name="doNumber">The DO Number of the job to download.</param>
        /// <param name="path">The path where the file will be stored. Starting
        /// point is the location of the source code.</param>
        /// <param name="document">"shipping-label" or "pod".</param>
        /// <param name="date">The date of the job to download.</param>
        public static async Task DownloadJobExport(
          string doNumber,
          string path,
          string document = "pod",
          string date = null)
        {
            IsAPIEmpty(DefaultApiKey);
            doNumber = HttpUtility.UrlEncode(doNumber);
            string link = $"dn/jobs/export/{doNumber}";
            if (date != null)
            {
                DateChecker(date);
                link += $"/{date}";
            }
            if (document != "pod" && document != "shipping-label")
            {
                throw new InvalidParameterException("Document must be either pod or shipping-label");
            }
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://app.detrack.com/api/v2/{link}?format=pdf&document={document}"),
                Headers =
                {
                    {
                        "X-API-KEY", DefaultApiKey
                    },
                    {
                        HttpRequestHeader.ContentType.ToString(), "application/octet-stream"
                    }
                }
            };

            HttpResponseMessage response = await client.SendAsync(request);
            byte[] responseData = await response.Content.ReadAsByteArrayAsync();
            if (path != "")
            {
                path = $"{path}/detrack-{document}-{doNumber}";
            }
            else
            {
                path = $"detrack-{document}-{doNumber}";
            }
            
            File.WriteAllBytes(path, responseData);
        }

        /// <summary>
        /// Creates a job in the database. It is a non-static method which must
        /// be used together with a <see cref="T:Detrack.DetrackCore.Job" /> instance. This function's
        /// call must be awaited.
        /// </summary>
        public static async Task CreateJob(Job job)
        {
            IsAPIEmpty(DefaultApiKey);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://app.detrack.com/api/v2/dn/jobs"),
                Method = HttpMethod.Post,
                Headers =
                {
                    {
                        "X-API-KEY", DefaultApiKey
                    },
                    {
                        HttpRequestHeader.ContentType.ToString(), "application/json"
                    }
                },
                Content = new StringContent(ClassToJson(job),
                                            Encoding.Default,
                                            "application/json")
            };
            
            HttpResponseMessage response = await client.SendAsync(request);
            string responseData = await response.Content.ReadAsStringAsync();
            CheckForExceptions(responseData);
        }

        /// <summary>
        /// Updates a job accroding to the given DO Number and date. This is a non-static
        /// method which must be used together with a <see cref="T:Detrack.DetrackCore.Job" />
        /// instance. This function's call must be awaited.
        /// </summary>
        /// <param name="doNumber">The given DO Number for the job.</param>
        /// <param name="date">Used to indicate date in which the job is made.
        /// If <see langword="null" />, it will take the job with the latest date</param>
        public async Task UpdateJob(string doNumber = null, string date = null)
        {
            IsAPIEmpty(DefaultApiKey);
            string link = "dn/jobs";
            if (doNumber != null)
            {
                doNumber = HttpUtility.UrlEncode(doNumber);
                link += $"/{doNumber}";
            }
            else
            {
                doNumber = HttpUtility.UrlEncode(DONumber);
                link += $"/{doNumber}";
            }
            if (date != null)
            {
                DateChecker(date);
                link += $"/{date}";
            }
            Job retrievedJob = await RetrieveJob(HttpUtility.UrlDecode(doNumber), date);
            PropertyInfo[] properties = typeof(Job).GetProperties();
            foreach (var item in _modified_properties)
            {
                foreach (PropertyInfo property in properties)
                {
                    if (property.ToString().Split(' ')[1] == item)
                    {
                        var value = property.GetValue(this);
                        property.SetValue(retrievedJob, value);
                    }
                }
            }
            string updatedJob = ClassToJson(retrievedJob);


            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://app.detrack.com/api/v2/{link}"),
                Method = HttpMethod.Put,
                Headers =
                {
                    {
                        "X-API-KEY", DefaultApiKey
                    },
                    {
                        HttpRequestHeader.ContentType.ToString(), "application/json"
                    }
                },
                Content = new StringContent(updatedJob, Encoding.Default, "application/json")
            };

            HttpResponseMessage response = await client.SendAsync(request);
            string responseData = await response.Content.ReadAsStringAsync();
            CheckForExceptions(responseData);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            _modified_properties.Add(propertyName);
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        private static string ClassToJson(Job jobClass)
        {
            return "{  \"data\": " + JsonConvert.SerializeObject(jobClass, Formatting.Indented) + "}";
        }

        private static string ClassToJsonList(List<Job> joblist)
        {
            return "{  \"data\": " + $"{JsonConvert.SerializeObject(joblist)}" + "}";
        }

        private static bool DateChecker(string dates)
        {
            MatchCollection matches = Regex.Matches(dates, "([0-9][0-9][0-9][0-9])-([0-1][0-9])-([0-3][0-9])");
            if (matches.Count != 0)
            {
                int month = int.Parse(matches[0].Value.Split('-')[1]);
                int day = int.Parse(matches[0].Value.Split('-')[2]);

                if (month > 12 || month <= 0)
                {
                    throw new InvalidDateException("Month is not a valid number");
                }

                if (day > 31 || day <= 0)
                {
                    throw new InvalidDateException("Day is not a valid number");
                }
                return true;
            }
            throw new InvalidDateException("Invalid date format. Date must be integer and date format must be yyyy-mm-dd");
        }

        private static void CheckForExceptions(string responseData)
        {
            if (responseData != string.Empty)
            {
                JObject jobject = JObject.Parse(responseData);
                if (jobject["code"] != null)
                {
                    if (jobject["code"].ToString() == "validation_failed")
                    {
                        string field = jobject["errors"][0]["field"].ToString();
                        string errors = jobject["errors"][0]["codes"][0].ToString();
                        field = ToPascal(field);
                        throw new ArgumentException($"{field} {errors}");
                    }

                    if (jobject["code"].ToString() == "forbidden")
                    {
                        throw new UndeletableJobException("Job is either completed, partially completed or failed.");
                    }
                        
                    if (jobject["code"].ToString() == "not_found")
                    {
                        throw new DONumberNotFoundException("Could not find job with this DO Number");
                    }
                        
                    if (jobject["code"].ToString() == "invalid_reattempt")
                    {
                        throw new InvalidReattemptException("Job must only be failed for reattempt.");
                    }
                        
                    if (jobject["code"].ToString() != null)
                    {
                        throw new ArgumentException(jobject["message"].ToString());
                    }   
                }
            }
        }

        private static void CheckForExceptionsInBatches(string responseData)
        {
            if (responseData != string.Empty)
            {
                JObject jobject = JObject.Parse(responseData);
                if (jobject["code"] != null)
                {
                    if (jobject["code"].ToString() == "validation_failed")
                    {
                        foreach (var item in jobject["errors"])
                        {
                            string field = item["errors"]?[0]["field"].ToString();
                            string errors = item["errors"]?[0]["codes"][0].ToString();
                            field = ToPascal(field);
                            throw new ArgumentException($"{field}: {item["do_number"]} {errors}");
                        }
                    }
                    else
                    {
                        switch (jobject["code"].ToString())
                        {
                            case "invalid_data":
                                throw new EmptyDataException("JobClass list is empty.");
                            case "deletion_failed":
                            {
                                foreach (var item in jobject["errors"])
                                {
                                    switch (item["codes"][0].ToString())
                                    {
                                        case "undeletable":
                                            throw new UndeletableJobException($"DO Number: {item["do_number"]} is either completed, partially completed or failed so it cannot be deleted.");
                                        case "not_found":
                                            throw new DONumberNotFoundException($"DO Number: {item["do_number"]}. Could not find job with this DO Number.");
                                    }
                                }
                                break;
                            }
                            case "update_failed":
                            {
                                foreach (var item in jobject["errors"])
                                {
                                    switch (item["codes"][0].ToString())
                                    {
                                        case "cannot be blank":
                                            throw new PODTimeEmptyException($"DO Number: {item["do_number"]}. {ToPascal(item["field"].ToString())} cannot be blank");
                                        case "not_found":
                                            throw new DONumberNotFoundException($"DO Number: {item["do_number"]}. Could not find job with this DO Number.");
                                        case "uneditable":
                                            throw new InvalidUpdateException($"DO Number: {item["do_number"]}. Unable to update job status as it is already completed/partially completed/failed");
                                    }
                                }
                                break;
                            }
                            default:
                            {
                                if (jobject["code"].ToString() != null)
                                {
                                    throw new ArgumentException(jobject["message"].ToString());
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        private static string ToPascal(string str)
        {
            return str.Split(new[] { "_" },
                    StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                    .Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }

        private static string ToSnake(string str)
        {
            switch (str)
            {
                case "DONumber":
                    return "do_number";
                case "PODTime":
                    return "pod_time";
                default:
                    return Regex.Replace(str, "([a-z])([A-Z])", "$1_$2").ToLower();
            }
        }

        private static void IsAPIEmpty(string APIKey)
        {
            if (string.IsNullOrEmpty(APIKey))
                throw new ArgumentException("API Key cannot be empty!");
        }

        private static string ClassToJsonListUpdate(List<Dictionary<string, object>> dictList)
        {
            List<string> stringList = new List<string>();
            foreach (Dictionary<string, object> dict in dictList)
            {
                string str = "{\n\"do_number\":" +
                             $"\"{dict["do_number"]}\"" +
                             ",\n" + "\"date\":" +
                             $"\"{dict["date"]}\"" + ",\n" +
                             "\"data\":" +
                             JsonConvert.SerializeObject(dict, (Formatting)1) +
                             "}\n";
                stringList.Add(str);
            }
            return "{\n\"data\": [" + string.Join(",", stringList.ToArray()) + "]\n}\n";
        }
    }
}

