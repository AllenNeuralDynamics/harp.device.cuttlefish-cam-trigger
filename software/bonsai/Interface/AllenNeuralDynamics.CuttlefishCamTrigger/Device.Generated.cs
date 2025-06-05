using Bonsai;
using Bonsai.Harp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Xml.Serialization;

namespace AllenNeuralDynamics.CuttlefishCamTrigger
{
    /// <summary>
    /// Generates events and processes commands for the CuttlefishCamTrigger device connected
    /// at the specified serial port.
    /// </summary>
    [Combinator(MethodName = nameof(Generate))]
    [WorkflowElementCategory(ElementCategory.Source)]
    [Description("Generates events and processes commands for the CuttlefishCamTrigger device.")]
    public partial class Device : Bonsai.Harp.Device, INamedElement
    {
        /// <summary>
        /// Represents the unique identity class of the <see cref="CuttlefishCamTrigger"/> device.
        /// This field is constant.
        /// </summary>
        public const int WhoAmI = 1408;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        public Device() : base(WhoAmI) { }

        string INamedElement.Name => nameof(CuttlefishCamTrigger);

        /// <summary>
        /// Gets a read-only mapping from address to register type.
        /// </summary>
        public static new IReadOnlyDictionary<int, Type> RegisterMap { get; } = new Dictionary<int, Type>
            (Bonsai.Harp.Device.RegisterMap.ToDictionary(entry => entry.Key, entry => entry.Value))
        {
            { 32, typeof(PwmEnabled) },
            { 33, typeof(PwmSet) },
            { 34, typeof(PwmClear) },
            { 35, typeof(PwmInvert) },
            { 36, typeof(RisingEdgeEventEnabled) },
            { 37, typeof(RisingEdgeEvent) },
            { 38, typeof(Pwm0FrequencyHz) },
            { 39, typeof(Pwm0DutyCycle) },
            { 40, typeof(Pwm1FrequencyHz) },
            { 41, typeof(Pwm1DutyCycle) },
            { 42, typeof(Pwm2FrequencyHz) },
            { 43, typeof(Pwm2DutyCycle) },
            { 44, typeof(Pwm3FrequencyHz) },
            { 45, typeof(Pwm3DutyCycle) },
            { 46, typeof(Pwm4FrequencyHz) },
            { 47, typeof(Pwm4DutyCycle) },
            { 48, typeof(Pwm5FrequencyHz) },
            { 49, typeof(Pwm5DutyCycle) },
            { 50, typeof(Pwm6FrequencyHz) },
            { 51, typeof(Pwm6DutyCycle) },
            { 52, typeof(Pwm7FrequencyHz) },
            { 53, typeof(Pwm7DutyCycle) }
        };

        /// <summary>
        /// Gets the contents of the metadata file describing the <see cref="CuttlefishCamTrigger"/>
        /// device registers.
        /// </summary>
        public static readonly string Metadata = GetDeviceMetadata();

        static string GetDeviceMetadata()
        {
            var deviceType = typeof(Device);
            using var metadataStream = deviceType.Assembly.GetManifestResourceStream($"{deviceType.Namespace}.device.yml");
            using var streamReader = new System.IO.StreamReader(metadataStream);
            return streamReader.ReadToEnd();
        }
    }

    /// <summary>
    /// Represents an operator that returns the contents of the metadata file
    /// describing the <see cref="CuttlefishCamTrigger"/> device registers.
    /// </summary>
    [Description("Returns the contents of the metadata file describing the CuttlefishCamTrigger device registers.")]
    public partial class GetDeviceMetadata : Source<string>
    {
        /// <summary>
        /// Returns an observable sequence with the contents of the metadata file
        /// describing the <see cref="CuttlefishCamTrigger"/> device registers.
        /// </summary>
        /// <returns>
        /// A sequence with a single <see cref="string"/> object representing the
        /// contents of the metadata file.
        /// </returns>
        public override IObservable<string> Generate()
        {
            return Observable.Return(Device.Metadata);
        }
    }

    /// <summary>
    /// Represents an operator that groups the sequence of <see cref="CuttlefishCamTrigger"/>" messages by register type.
    /// </summary>
    [Description("Groups the sequence of CuttlefishCamTrigger messages by register type.")]
    public partial class GroupByRegister : Combinator<HarpMessage, IGroupedObservable<Type, HarpMessage>>
    {
        /// <summary>
        /// Groups an observable sequence of <see cref="CuttlefishCamTrigger"/> messages
        /// by register type.
        /// </summary>
        /// <param name="source">The sequence of Harp device messages.</param>
        /// <returns>
        /// A sequence of observable groups, each of which corresponds to a unique
        /// <see cref="CuttlefishCamTrigger"/> register.
        /// </returns>
        public override IObservable<IGroupedObservable<Type, HarpMessage>> Process(IObservable<HarpMessage> source)
        {
            return source.GroupBy(message => Device.RegisterMap[message.Address]);
        }
    }

    /// <summary>
    /// Represents an operator that writes the sequence of <see cref="CuttlefishCamTrigger"/>" messages
    /// to the standard Harp storage format.
    /// </summary>
    [Description("Writes the sequence of CuttlefishCamTrigger messages to the standard Harp storage format.")]
    public partial class DeviceDataWriter : Sink<HarpMessage>, INamedElement
    {
        const string BinaryExtension = ".bin";
        const string MetadataFileName = "device.yml";
        readonly Bonsai.Harp.MessageWriter writer = new();

        string INamedElement.Name => nameof(CuttlefishCamTrigger) + "DataWriter";

        /// <summary>
        /// Gets or sets the relative or absolute path on which to save the message data.
        /// </summary>
        [Description("The relative or absolute path of the directory on which to save the message data.")]
        [Editor("Bonsai.Design.SaveFileNameEditor, Bonsai.Design", DesignTypes.UITypeEditor)]
        public string Path
        {
            get => System.IO.Path.GetDirectoryName(writer.FileName);
            set => writer.FileName = System.IO.Path.Combine(value, nameof(CuttlefishCamTrigger) + BinaryExtension);
        }

        /// <summary>
        /// Gets or sets a value indicating whether element writing should be buffered. If <see langword="true"/>,
        /// the write commands will be queued in memory as fast as possible and will be processed
        /// by the writer in a different thread. Otherwise, writing will be done in the same
        /// thread in which notifications arrive.
        /// </summary>
        [Description("Indicates whether writing should be buffered.")]
        public bool Buffered
        {
            get => writer.Buffered;
            set => writer.Buffered = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to overwrite the output file if it already exists.
        /// </summary>
        [Description("Indicates whether to overwrite the output file if it already exists.")]
        public bool Overwrite
        {
            get => writer.Overwrite;
            set => writer.Overwrite = value;
        }

        /// <summary>
        /// Gets or sets a value specifying how the message filter will use the matching criteria.
        /// </summary>
        [Description("Specifies how the message filter will use the matching criteria.")]
        public FilterType FilterType
        {
            get => writer.FilterType;
            set => writer.FilterType = value;
        }

        /// <summary>
        /// Gets or sets a value specifying the expected message type. If no value is
        /// specified, all messages will be accepted.
        /// </summary>
        [Description("Specifies the expected message type. If no value is specified, all messages will be accepted.")]
        public MessageType? MessageType
        {
            get => writer.MessageType;
            set => writer.MessageType = value;
        }

        private IObservable<TSource> WriteDeviceMetadata<TSource>(IObservable<TSource> source)
        {
            var basePath = Path;
            if (string.IsNullOrEmpty(basePath))
                return source;

            var metadataPath = System.IO.Path.Combine(basePath, MetadataFileName);
            return Observable.Create<TSource>(observer =>
            {
                Bonsai.IO.PathHelper.EnsureDirectory(metadataPath);
                if (System.IO.File.Exists(metadataPath) && !Overwrite)
                {
                    throw new System.IO.IOException(string.Format("The file '{0}' already exists.", metadataPath));
                }

                System.IO.File.WriteAllText(metadataPath, Device.Metadata);
                return source.SubscribeSafe(observer);
            });
        }

        /// <summary>
        /// Writes each Harp message in the sequence to the specified binary file, and the
        /// contents of the device metadata file to a separate text file.
        /// </summary>
        /// <param name="source">The sequence of messages to write to the file.</param>
        /// <returns>
        /// An observable sequence that is identical to the <paramref name="source"/>
        /// sequence but where there is an additional side effect of writing the
        /// messages to a raw binary file, and the contents of the device metadata file
        /// to a separate text file.
        /// </returns>
        public override IObservable<HarpMessage> Process(IObservable<HarpMessage> source)
        {
            return source.Publish(ps => ps.Merge(
                WriteDeviceMetadata(writer.Process(ps.GroupBy(message => message.Address)))
                .IgnoreElements()
                .Cast<HarpMessage>()));
        }

        /// <summary>
        /// Writes each Harp message in the sequence of observable groups to the
        /// corresponding binary file, where the name of each file is generated from
        /// the common group register address. The contents of the device metadata file are
        /// written to a separate text file.
        /// </summary>
        /// <param name="source">
        /// A sequence of observable groups, each of which corresponds to a unique register
        /// address.
        /// </param>
        /// <returns>
        /// An observable sequence that is identical to the <paramref name="source"/>
        /// sequence but where there is an additional side effect of writing the Harp
        /// messages in each group to the corresponding file, and the contents of the device
        /// metadata file to a separate text file.
        /// </returns>
        public IObservable<IGroupedObservable<int, HarpMessage>> Process(IObservable<IGroupedObservable<int, HarpMessage>> source)
        {
            return WriteDeviceMetadata(writer.Process(source));
        }

        /// <summary>
        /// Writes each Harp message in the sequence of observable groups to the
        /// corresponding binary file, where the name of each file is generated from
        /// the common group register name. The contents of the device metadata file are
        /// written to a separate text file.
        /// </summary>
        /// <param name="source">
        /// A sequence of observable groups, each of which corresponds to a unique register
        /// type.
        /// </param>
        /// <returns>
        /// An observable sequence that is identical to the <paramref name="source"/>
        /// sequence but where there is an additional side effect of writing the Harp
        /// messages in each group to the corresponding file, and the contents of the device
        /// metadata file to a separate text file.
        /// </returns>
        public IObservable<IGroupedObservable<Type, HarpMessage>> Process(IObservable<IGroupedObservable<Type, HarpMessage>> source)
        {
            return WriteDeviceMetadata(writer.Process(source));
        }
    }

    /// <summary>
    /// Represents an operator that filters register-specific messages
    /// reported by the <see cref="CuttlefishCamTrigger"/> device.
    /// </summary>
    /// <seealso cref="PwmEnabled"/>
    /// <seealso cref="PwmSet"/>
    /// <seealso cref="PwmClear"/>
    /// <seealso cref="PwmInvert"/>
    /// <seealso cref="RisingEdgeEventEnabled"/>
    /// <seealso cref="RisingEdgeEvent"/>
    /// <seealso cref="Pwm0FrequencyHz"/>
    /// <seealso cref="Pwm0DutyCycle"/>
    /// <seealso cref="Pwm1FrequencyHz"/>
    /// <seealso cref="Pwm1DutyCycle"/>
    /// <seealso cref="Pwm2FrequencyHz"/>
    /// <seealso cref="Pwm2DutyCycle"/>
    /// <seealso cref="Pwm3FrequencyHz"/>
    /// <seealso cref="Pwm3DutyCycle"/>
    /// <seealso cref="Pwm4FrequencyHz"/>
    /// <seealso cref="Pwm4DutyCycle"/>
    /// <seealso cref="Pwm5FrequencyHz"/>
    /// <seealso cref="Pwm5DutyCycle"/>
    /// <seealso cref="Pwm6FrequencyHz"/>
    /// <seealso cref="Pwm6DutyCycle"/>
    /// <seealso cref="Pwm7FrequencyHz"/>
    /// <seealso cref="Pwm7DutyCycle"/>
    [XmlInclude(typeof(PwmEnabled))]
    [XmlInclude(typeof(PwmSet))]
    [XmlInclude(typeof(PwmClear))]
    [XmlInclude(typeof(PwmInvert))]
    [XmlInclude(typeof(RisingEdgeEventEnabled))]
    [XmlInclude(typeof(RisingEdgeEvent))]
    [XmlInclude(typeof(Pwm0FrequencyHz))]
    [XmlInclude(typeof(Pwm0DutyCycle))]
    [XmlInclude(typeof(Pwm1FrequencyHz))]
    [XmlInclude(typeof(Pwm1DutyCycle))]
    [XmlInclude(typeof(Pwm2FrequencyHz))]
    [XmlInclude(typeof(Pwm2DutyCycle))]
    [XmlInclude(typeof(Pwm3FrequencyHz))]
    [XmlInclude(typeof(Pwm3DutyCycle))]
    [XmlInclude(typeof(Pwm4FrequencyHz))]
    [XmlInclude(typeof(Pwm4DutyCycle))]
    [XmlInclude(typeof(Pwm5FrequencyHz))]
    [XmlInclude(typeof(Pwm5DutyCycle))]
    [XmlInclude(typeof(Pwm6FrequencyHz))]
    [XmlInclude(typeof(Pwm6DutyCycle))]
    [XmlInclude(typeof(Pwm7FrequencyHz))]
    [XmlInclude(typeof(Pwm7DutyCycle))]
    [Description("Filters register-specific messages reported by the CuttlefishCamTrigger device.")]
    public class FilterRegister : FilterRegisterBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterRegister"/> class.
        /// </summary>
        public FilterRegister()
        {
            Register = new PwmEnabled();
        }

        string INamedElement.Name
        {
            get => $"{nameof(CuttlefishCamTrigger)}.{GetElementDisplayName(Register)}";
        }
    }

    /// <summary>
    /// Represents an operator which filters and selects specific messages
    /// reported by the CuttlefishCamTrigger device.
    /// </summary>
    /// <seealso cref="PwmEnabled"/>
    /// <seealso cref="PwmSet"/>
    /// <seealso cref="PwmClear"/>
    /// <seealso cref="PwmInvert"/>
    /// <seealso cref="RisingEdgeEventEnabled"/>
    /// <seealso cref="RisingEdgeEvent"/>
    /// <seealso cref="Pwm0FrequencyHz"/>
    /// <seealso cref="Pwm0DutyCycle"/>
    /// <seealso cref="Pwm1FrequencyHz"/>
    /// <seealso cref="Pwm1DutyCycle"/>
    /// <seealso cref="Pwm2FrequencyHz"/>
    /// <seealso cref="Pwm2DutyCycle"/>
    /// <seealso cref="Pwm3FrequencyHz"/>
    /// <seealso cref="Pwm3DutyCycle"/>
    /// <seealso cref="Pwm4FrequencyHz"/>
    /// <seealso cref="Pwm4DutyCycle"/>
    /// <seealso cref="Pwm5FrequencyHz"/>
    /// <seealso cref="Pwm5DutyCycle"/>
    /// <seealso cref="Pwm6FrequencyHz"/>
    /// <seealso cref="Pwm6DutyCycle"/>
    /// <seealso cref="Pwm7FrequencyHz"/>
    /// <seealso cref="Pwm7DutyCycle"/>
    [XmlInclude(typeof(PwmEnabled))]
    [XmlInclude(typeof(PwmSet))]
    [XmlInclude(typeof(PwmClear))]
    [XmlInclude(typeof(PwmInvert))]
    [XmlInclude(typeof(RisingEdgeEventEnabled))]
    [XmlInclude(typeof(RisingEdgeEvent))]
    [XmlInclude(typeof(Pwm0FrequencyHz))]
    [XmlInclude(typeof(Pwm0DutyCycle))]
    [XmlInclude(typeof(Pwm1FrequencyHz))]
    [XmlInclude(typeof(Pwm1DutyCycle))]
    [XmlInclude(typeof(Pwm2FrequencyHz))]
    [XmlInclude(typeof(Pwm2DutyCycle))]
    [XmlInclude(typeof(Pwm3FrequencyHz))]
    [XmlInclude(typeof(Pwm3DutyCycle))]
    [XmlInclude(typeof(Pwm4FrequencyHz))]
    [XmlInclude(typeof(Pwm4DutyCycle))]
    [XmlInclude(typeof(Pwm5FrequencyHz))]
    [XmlInclude(typeof(Pwm5DutyCycle))]
    [XmlInclude(typeof(Pwm6FrequencyHz))]
    [XmlInclude(typeof(Pwm6DutyCycle))]
    [XmlInclude(typeof(Pwm7FrequencyHz))]
    [XmlInclude(typeof(Pwm7DutyCycle))]
    [XmlInclude(typeof(TimestampedPwmEnabled))]
    [XmlInclude(typeof(TimestampedPwmSet))]
    [XmlInclude(typeof(TimestampedPwmClear))]
    [XmlInclude(typeof(TimestampedPwmInvert))]
    [XmlInclude(typeof(TimestampedRisingEdgeEventEnabled))]
    [XmlInclude(typeof(TimestampedRisingEdgeEvent))]
    [XmlInclude(typeof(TimestampedPwm0FrequencyHz))]
    [XmlInclude(typeof(TimestampedPwm0DutyCycle))]
    [XmlInclude(typeof(TimestampedPwm1FrequencyHz))]
    [XmlInclude(typeof(TimestampedPwm1DutyCycle))]
    [XmlInclude(typeof(TimestampedPwm2FrequencyHz))]
    [XmlInclude(typeof(TimestampedPwm2DutyCycle))]
    [XmlInclude(typeof(TimestampedPwm3FrequencyHz))]
    [XmlInclude(typeof(TimestampedPwm3DutyCycle))]
    [XmlInclude(typeof(TimestampedPwm4FrequencyHz))]
    [XmlInclude(typeof(TimestampedPwm4DutyCycle))]
    [XmlInclude(typeof(TimestampedPwm5FrequencyHz))]
    [XmlInclude(typeof(TimestampedPwm5DutyCycle))]
    [XmlInclude(typeof(TimestampedPwm6FrequencyHz))]
    [XmlInclude(typeof(TimestampedPwm6DutyCycle))]
    [XmlInclude(typeof(TimestampedPwm7FrequencyHz))]
    [XmlInclude(typeof(TimestampedPwm7DutyCycle))]
    [Description("Filters and selects specific messages reported by the CuttlefishCamTrigger device.")]
    public partial class Parse : ParseBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Parse"/> class.
        /// </summary>
        public Parse()
        {
            Register = new PwmEnabled();
        }

        string INamedElement.Name => $"{nameof(CuttlefishCamTrigger)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents an operator which formats a sequence of values as specific
    /// CuttlefishCamTrigger register messages.
    /// </summary>
    /// <seealso cref="PwmEnabled"/>
    /// <seealso cref="PwmSet"/>
    /// <seealso cref="PwmClear"/>
    /// <seealso cref="PwmInvert"/>
    /// <seealso cref="RisingEdgeEventEnabled"/>
    /// <seealso cref="RisingEdgeEvent"/>
    /// <seealso cref="Pwm0FrequencyHz"/>
    /// <seealso cref="Pwm0DutyCycle"/>
    /// <seealso cref="Pwm1FrequencyHz"/>
    /// <seealso cref="Pwm1DutyCycle"/>
    /// <seealso cref="Pwm2FrequencyHz"/>
    /// <seealso cref="Pwm2DutyCycle"/>
    /// <seealso cref="Pwm3FrequencyHz"/>
    /// <seealso cref="Pwm3DutyCycle"/>
    /// <seealso cref="Pwm4FrequencyHz"/>
    /// <seealso cref="Pwm4DutyCycle"/>
    /// <seealso cref="Pwm5FrequencyHz"/>
    /// <seealso cref="Pwm5DutyCycle"/>
    /// <seealso cref="Pwm6FrequencyHz"/>
    /// <seealso cref="Pwm6DutyCycle"/>
    /// <seealso cref="Pwm7FrequencyHz"/>
    /// <seealso cref="Pwm7DutyCycle"/>
    [XmlInclude(typeof(PwmEnabled))]
    [XmlInclude(typeof(PwmSet))]
    [XmlInclude(typeof(PwmClear))]
    [XmlInclude(typeof(PwmInvert))]
    [XmlInclude(typeof(RisingEdgeEventEnabled))]
    [XmlInclude(typeof(RisingEdgeEvent))]
    [XmlInclude(typeof(Pwm0FrequencyHz))]
    [XmlInclude(typeof(Pwm0DutyCycle))]
    [XmlInclude(typeof(Pwm1FrequencyHz))]
    [XmlInclude(typeof(Pwm1DutyCycle))]
    [XmlInclude(typeof(Pwm2FrequencyHz))]
    [XmlInclude(typeof(Pwm2DutyCycle))]
    [XmlInclude(typeof(Pwm3FrequencyHz))]
    [XmlInclude(typeof(Pwm3DutyCycle))]
    [XmlInclude(typeof(Pwm4FrequencyHz))]
    [XmlInclude(typeof(Pwm4DutyCycle))]
    [XmlInclude(typeof(Pwm5FrequencyHz))]
    [XmlInclude(typeof(Pwm5DutyCycle))]
    [XmlInclude(typeof(Pwm6FrequencyHz))]
    [XmlInclude(typeof(Pwm6DutyCycle))]
    [XmlInclude(typeof(Pwm7FrequencyHz))]
    [XmlInclude(typeof(Pwm7DutyCycle))]
    [Description("Formats a sequence of values as specific CuttlefishCamTrigger register messages.")]
    public partial class Format : FormatBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Format"/> class.
        /// </summary>
        public Format()
        {
            Register = new PwmEnabled();
        }

        string INamedElement.Name => $"{nameof(CuttlefishCamTrigger)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents a register that bitmask to enable/disable each of the 8 Pwm outputs.
    /// </summary>
    [Description("Bitmask to enable/disable each of the 8 Pwm outputs")]
    public partial class PwmEnabled
    {
        /// <summary>
        /// Represents the address of the <see cref="PwmEnabled"/> register. This field is constant.
        /// </summary>
        public const int Address = 32;

        /// <summary>
        /// Represents the payload type of the <see cref="PwmEnabled"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="PwmEnabled"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="PwmEnabled"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static Ports GetPayload(HarpMessage message)
        {
            return (Ports)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="PwmEnabled"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Ports> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((Ports)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="PwmEnabled"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PwmEnabled"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, Ports value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="PwmEnabled"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PwmEnabled"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, Ports value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// PwmEnabled register.
    /// </summary>
    /// <seealso cref="PwmEnabled"/>
    [Description("Filters and selects timestamped messages from the PwmEnabled register.")]
    public partial class TimestampedPwmEnabled
    {
        /// <summary>
        /// Represents the address of the <see cref="PwmEnabled"/> register. This field is constant.
        /// </summary>
        public const int Address = PwmEnabled.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="PwmEnabled"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Ports> GetPayload(HarpMessage message)
        {
            return PwmEnabled.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that bitmask to enable any of the 8 Pwm outputs when corresponding bit is set to 1.
    /// </summary>
    [Description("Bitmask to enable any of the 8 Pwm outputs when corresponding bit is set to 1.")]
    public partial class PwmSet
    {
        /// <summary>
        /// Represents the address of the <see cref="PwmSet"/> register. This field is constant.
        /// </summary>
        public const int Address = 33;

        /// <summary>
        /// Represents the payload type of the <see cref="PwmSet"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="PwmSet"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="PwmSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static Ports GetPayload(HarpMessage message)
        {
            return (Ports)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="PwmSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Ports> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((Ports)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="PwmSet"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PwmSet"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, Ports value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="PwmSet"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PwmSet"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, Ports value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// PwmSet register.
    /// </summary>
    /// <seealso cref="PwmSet"/>
    [Description("Filters and selects timestamped messages from the PwmSet register.")]
    public partial class TimestampedPwmSet
    {
        /// <summary>
        /// Represents the address of the <see cref="PwmSet"/> register. This field is constant.
        /// </summary>
        public const int Address = PwmSet.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="PwmSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Ports> GetPayload(HarpMessage message)
        {
            return PwmSet.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that bitmask to disable any of the 8 Pwm outputs when corresponding bit is set to 1.
    /// </summary>
    [Description("Bitmask to disable any of the 8 Pwm outputs when corresponding bit is set to 1.")]
    public partial class PwmClear
    {
        /// <summary>
        /// Represents the address of the <see cref="PwmClear"/> register. This field is constant.
        /// </summary>
        public const int Address = 34;

        /// <summary>
        /// Represents the payload type of the <see cref="PwmClear"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="PwmClear"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="PwmClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static Ports GetPayload(HarpMessage message)
        {
            return (Ports)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="PwmClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Ports> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((Ports)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="PwmClear"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PwmClear"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, Ports value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="PwmClear"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PwmClear"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, Ports value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// PwmClear register.
    /// </summary>
    /// <seealso cref="PwmClear"/>
    [Description("Filters and selects timestamped messages from the PwmClear register.")]
    public partial class TimestampedPwmClear
    {
        /// <summary>
        /// Represents the address of the <see cref="PwmClear"/> register. This field is constant.
        /// </summary>
        public const int Address = PwmClear.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="PwmClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Ports> GetPayload(HarpMessage message)
        {
            return PwmClear.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that bitmask to invert each of the 8 Pwm outputs if set to 1.
    /// </summary>
    [Description("Bitmask to invert each of the 8 Pwm outputs if set to 1")]
    public partial class PwmInvert
    {
        /// <summary>
        /// Represents the address of the <see cref="PwmInvert"/> register. This field is constant.
        /// </summary>
        public const int Address = 35;

        /// <summary>
        /// Represents the payload type of the <see cref="PwmInvert"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="PwmInvert"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="PwmInvert"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static Ports GetPayload(HarpMessage message)
        {
            return (Ports)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="PwmInvert"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Ports> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((Ports)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="PwmInvert"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PwmInvert"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, Ports value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="PwmInvert"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PwmInvert"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, Ports value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// PwmInvert register.
    /// </summary>
    /// <seealso cref="PwmInvert"/>
    [Description("Filters and selects timestamped messages from the PwmInvert register.")]
    public partial class TimestampedPwmInvert
    {
        /// <summary>
        /// Represents the address of the <see cref="PwmInvert"/> register. This field is constant.
        /// </summary>
        public const int Address = PwmInvert.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="PwmInvert"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Ports> GetPayload(HarpMessage message)
        {
            return PwmInvert.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that bitmask to enable/disable dispatch of a rising edge event message for each of the corresponding Pwm outputs.
    /// </summary>
    [Description("Bitmask to enable/disable dispatch of a rising edge event message for each of the corresponding Pwm outputs")]
    public partial class RisingEdgeEventEnabled
    {
        /// <summary>
        /// Represents the address of the <see cref="RisingEdgeEventEnabled"/> register. This field is constant.
        /// </summary>
        public const int Address = 36;

        /// <summary>
        /// Represents the payload type of the <see cref="RisingEdgeEventEnabled"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="RisingEdgeEventEnabled"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="RisingEdgeEventEnabled"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="RisingEdgeEventEnabled"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="RisingEdgeEventEnabled"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="RisingEdgeEventEnabled"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="RisingEdgeEventEnabled"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="RisingEdgeEventEnabled"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// RisingEdgeEventEnabled register.
    /// </summary>
    /// <seealso cref="RisingEdgeEventEnabled"/>
    [Description("Filters and selects timestamped messages from the RisingEdgeEventEnabled register.")]
    public partial class TimestampedRisingEdgeEventEnabled
    {
        /// <summary>
        /// Represents the address of the <see cref="RisingEdgeEventEnabled"/> register. This field is constant.
        /// </summary>
        public const int Address = RisingEdgeEventEnabled.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="RisingEdgeEventEnabled"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return RisingEdgeEventEnabled.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that bitmask with the current state of the Pwm outputs. This event is dispatched if any of the specified outputs sees a rising edge.
    /// </summary>
    [Description("Bitmask with the current state of the Pwm outputs. This event is dispatched if any of the specified outputs sees a rising edge.")]
    public partial class RisingEdgeEvent
    {
        /// <summary>
        /// Represents the address of the <see cref="RisingEdgeEvent"/> register. This field is constant.
        /// </summary>
        public const int Address = 37;

        /// <summary>
        /// Represents the payload type of the <see cref="RisingEdgeEvent"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="RisingEdgeEvent"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="RisingEdgeEvent"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="RisingEdgeEvent"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="RisingEdgeEvent"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="RisingEdgeEvent"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="RisingEdgeEvent"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="RisingEdgeEvent"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// RisingEdgeEvent register.
    /// </summary>
    /// <seealso cref="RisingEdgeEvent"/>
    [Description("Filters and selects timestamped messages from the RisingEdgeEvent register.")]
    public partial class TimestampedRisingEdgeEvent
    {
        /// <summary>
        /// Represents the address of the <see cref="RisingEdgeEvent"/> register. This field is constant.
        /// </summary>
        public const int Address = RisingEdgeEvent.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="RisingEdgeEvent"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return RisingEdgeEvent.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pwm output 0 frequency setting in Hz.
    /// </summary>
    [Description("Pwm output 0 frequency setting in Hz.")]
    public partial class Pwm0FrequencyHz
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm0FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int Address = 38;

        /// <summary>
        /// Represents the payload type of the <see cref="Pwm0FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Pwm0FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Pwm0FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Pwm0FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Pwm0FrequencyHz"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm0FrequencyHz"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Pwm0FrequencyHz"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm0FrequencyHz"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Pwm0FrequencyHz register.
    /// </summary>
    /// <seealso cref="Pwm0FrequencyHz"/>
    [Description("Filters and selects timestamped messages from the Pwm0FrequencyHz register.")]
    public partial class TimestampedPwm0FrequencyHz
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm0FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int Address = Pwm0FrequencyHz.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Pwm0FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Pwm0FrequencyHz.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pwm output 0 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [Description("Pwm output 0 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class Pwm0DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm0DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = 39;

        /// <summary>
        /// Represents the payload type of the <see cref="Pwm0DutyCycle"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Pwm0DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Pwm0DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Pwm0DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Pwm0DutyCycle"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm0DutyCycle"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Pwm0DutyCycle"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm0DutyCycle"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Pwm0DutyCycle register.
    /// </summary>
    /// <seealso cref="Pwm0DutyCycle"/>
    [Description("Filters and selects timestamped messages from the Pwm0DutyCycle register.")]
    public partial class TimestampedPwm0DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm0DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = Pwm0DutyCycle.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Pwm0DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Pwm0DutyCycle.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pwm output 1 frequency setting in Hz.
    /// </summary>
    [Description("Pwm output 1 frequency setting in Hz.")]
    public partial class Pwm1FrequencyHz
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm1FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int Address = 40;

        /// <summary>
        /// Represents the payload type of the <see cref="Pwm1FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Pwm1FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Pwm1FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Pwm1FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Pwm1FrequencyHz"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm1FrequencyHz"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Pwm1FrequencyHz"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm1FrequencyHz"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Pwm1FrequencyHz register.
    /// </summary>
    /// <seealso cref="Pwm1FrequencyHz"/>
    [Description("Filters and selects timestamped messages from the Pwm1FrequencyHz register.")]
    public partial class TimestampedPwm1FrequencyHz
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm1FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int Address = Pwm1FrequencyHz.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Pwm1FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Pwm1FrequencyHz.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pwm output 1 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [Description("Pwm output 1 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class Pwm1DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm1DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = 41;

        /// <summary>
        /// Represents the payload type of the <see cref="Pwm1DutyCycle"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Pwm1DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Pwm1DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Pwm1DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Pwm1DutyCycle"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm1DutyCycle"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Pwm1DutyCycle"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm1DutyCycle"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Pwm1DutyCycle register.
    /// </summary>
    /// <seealso cref="Pwm1DutyCycle"/>
    [Description("Filters and selects timestamped messages from the Pwm1DutyCycle register.")]
    public partial class TimestampedPwm1DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm1DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = Pwm1DutyCycle.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Pwm1DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Pwm1DutyCycle.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pwm output 2 frequency setting in Hz.
    /// </summary>
    [Description("Pwm output 2 frequency setting in Hz.")]
    public partial class Pwm2FrequencyHz
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm2FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int Address = 42;

        /// <summary>
        /// Represents the payload type of the <see cref="Pwm2FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Pwm2FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Pwm2FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Pwm2FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Pwm2FrequencyHz"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm2FrequencyHz"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Pwm2FrequencyHz"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm2FrequencyHz"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Pwm2FrequencyHz register.
    /// </summary>
    /// <seealso cref="Pwm2FrequencyHz"/>
    [Description("Filters and selects timestamped messages from the Pwm2FrequencyHz register.")]
    public partial class TimestampedPwm2FrequencyHz
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm2FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int Address = Pwm2FrequencyHz.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Pwm2FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Pwm2FrequencyHz.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pwm output 2 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [Description("Pwm output 2 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class Pwm2DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm2DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = 43;

        /// <summary>
        /// Represents the payload type of the <see cref="Pwm2DutyCycle"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Pwm2DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Pwm2DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Pwm2DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Pwm2DutyCycle"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm2DutyCycle"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Pwm2DutyCycle"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm2DutyCycle"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Pwm2DutyCycle register.
    /// </summary>
    /// <seealso cref="Pwm2DutyCycle"/>
    [Description("Filters and selects timestamped messages from the Pwm2DutyCycle register.")]
    public partial class TimestampedPwm2DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm2DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = Pwm2DutyCycle.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Pwm2DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Pwm2DutyCycle.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pwm output 3 frequency setting in Hz.
    /// </summary>
    [Description("Pwm output 3 frequency setting in Hz.")]
    public partial class Pwm3FrequencyHz
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm3FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int Address = 44;

        /// <summary>
        /// Represents the payload type of the <see cref="Pwm3FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Pwm3FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Pwm3FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Pwm3FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Pwm3FrequencyHz"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm3FrequencyHz"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Pwm3FrequencyHz"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm3FrequencyHz"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Pwm3FrequencyHz register.
    /// </summary>
    /// <seealso cref="Pwm3FrequencyHz"/>
    [Description("Filters and selects timestamped messages from the Pwm3FrequencyHz register.")]
    public partial class TimestampedPwm3FrequencyHz
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm3FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int Address = Pwm3FrequencyHz.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Pwm3FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Pwm3FrequencyHz.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pwm output 3 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [Description("Pwm output 3 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class Pwm3DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm3DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = 45;

        /// <summary>
        /// Represents the payload type of the <see cref="Pwm3DutyCycle"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Pwm3DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Pwm3DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Pwm3DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Pwm3DutyCycle"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm3DutyCycle"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Pwm3DutyCycle"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm3DutyCycle"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Pwm3DutyCycle register.
    /// </summary>
    /// <seealso cref="Pwm3DutyCycle"/>
    [Description("Filters and selects timestamped messages from the Pwm3DutyCycle register.")]
    public partial class TimestampedPwm3DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm3DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = Pwm3DutyCycle.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Pwm3DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Pwm3DutyCycle.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pwm output 4 frequency setting in Hz.
    /// </summary>
    [Description("Pwm output 4 frequency setting in Hz.")]
    public partial class Pwm4FrequencyHz
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm4FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int Address = 46;

        /// <summary>
        /// Represents the payload type of the <see cref="Pwm4FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Pwm4FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Pwm4FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Pwm4FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Pwm4FrequencyHz"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm4FrequencyHz"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Pwm4FrequencyHz"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm4FrequencyHz"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Pwm4FrequencyHz register.
    /// </summary>
    /// <seealso cref="Pwm4FrequencyHz"/>
    [Description("Filters and selects timestamped messages from the Pwm4FrequencyHz register.")]
    public partial class TimestampedPwm4FrequencyHz
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm4FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int Address = Pwm4FrequencyHz.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Pwm4FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Pwm4FrequencyHz.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pwm output 4 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [Description("Pwm output 4 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class Pwm4DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm4DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = 47;

        /// <summary>
        /// Represents the payload type of the <see cref="Pwm4DutyCycle"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Pwm4DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Pwm4DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Pwm4DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Pwm4DutyCycle"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm4DutyCycle"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Pwm4DutyCycle"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm4DutyCycle"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Pwm4DutyCycle register.
    /// </summary>
    /// <seealso cref="Pwm4DutyCycle"/>
    [Description("Filters and selects timestamped messages from the Pwm4DutyCycle register.")]
    public partial class TimestampedPwm4DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm4DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = Pwm4DutyCycle.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Pwm4DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Pwm4DutyCycle.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pwm output 5 frequency setting in Hz.
    /// </summary>
    [Description("Pwm output 5 frequency setting in Hz.")]
    public partial class Pwm5FrequencyHz
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm5FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int Address = 48;

        /// <summary>
        /// Represents the payload type of the <see cref="Pwm5FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Pwm5FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Pwm5FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Pwm5FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Pwm5FrequencyHz"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm5FrequencyHz"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Pwm5FrequencyHz"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm5FrequencyHz"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Pwm5FrequencyHz register.
    /// </summary>
    /// <seealso cref="Pwm5FrequencyHz"/>
    [Description("Filters and selects timestamped messages from the Pwm5FrequencyHz register.")]
    public partial class TimestampedPwm5FrequencyHz
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm5FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int Address = Pwm5FrequencyHz.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Pwm5FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Pwm5FrequencyHz.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pwm output 5 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [Description("Pwm output 5 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class Pwm5DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm5DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = 49;

        /// <summary>
        /// Represents the payload type of the <see cref="Pwm5DutyCycle"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Pwm5DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Pwm5DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Pwm5DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Pwm5DutyCycle"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm5DutyCycle"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Pwm5DutyCycle"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm5DutyCycle"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Pwm5DutyCycle register.
    /// </summary>
    /// <seealso cref="Pwm5DutyCycle"/>
    [Description("Filters and selects timestamped messages from the Pwm5DutyCycle register.")]
    public partial class TimestampedPwm5DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm5DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = Pwm5DutyCycle.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Pwm5DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Pwm5DutyCycle.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pwm output 6 frequency setting in Hz.
    /// </summary>
    [Description("Pwm output 6 frequency setting in Hz.")]
    public partial class Pwm6FrequencyHz
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm6FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int Address = 50;

        /// <summary>
        /// Represents the payload type of the <see cref="Pwm6FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Pwm6FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Pwm6FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Pwm6FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Pwm6FrequencyHz"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm6FrequencyHz"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Pwm6FrequencyHz"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm6FrequencyHz"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Pwm6FrequencyHz register.
    /// </summary>
    /// <seealso cref="Pwm6FrequencyHz"/>
    [Description("Filters and selects timestamped messages from the Pwm6FrequencyHz register.")]
    public partial class TimestampedPwm6FrequencyHz
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm6FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int Address = Pwm6FrequencyHz.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Pwm6FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Pwm6FrequencyHz.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pwm output 6 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [Description("Pwm output 6 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class Pwm6DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm6DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = 51;

        /// <summary>
        /// Represents the payload type of the <see cref="Pwm6DutyCycle"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Pwm6DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Pwm6DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Pwm6DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Pwm6DutyCycle"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm6DutyCycle"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Pwm6DutyCycle"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm6DutyCycle"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Pwm6DutyCycle register.
    /// </summary>
    /// <seealso cref="Pwm6DutyCycle"/>
    [Description("Filters and selects timestamped messages from the Pwm6DutyCycle register.")]
    public partial class TimestampedPwm6DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm6DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = Pwm6DutyCycle.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Pwm6DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Pwm6DutyCycle.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pwm output 7 frequency setting in Hz.
    /// </summary>
    [Description("Pwm output 7 frequency setting in Hz.")]
    public partial class Pwm7FrequencyHz
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm7FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int Address = 52;

        /// <summary>
        /// Represents the payload type of the <see cref="Pwm7FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Pwm7FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Pwm7FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Pwm7FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Pwm7FrequencyHz"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm7FrequencyHz"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Pwm7FrequencyHz"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm7FrequencyHz"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Pwm7FrequencyHz register.
    /// </summary>
    /// <seealso cref="Pwm7FrequencyHz"/>
    [Description("Filters and selects timestamped messages from the Pwm7FrequencyHz register.")]
    public partial class TimestampedPwm7FrequencyHz
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm7FrequencyHz"/> register. This field is constant.
        /// </summary>
        public const int Address = Pwm7FrequencyHz.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Pwm7FrequencyHz"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Pwm7FrequencyHz.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pwm output 7 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [Description("Pwm output 7 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class Pwm7DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm7DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = 53;

        /// <summary>
        /// Represents the payload type of the <see cref="Pwm7DutyCycle"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Pwm7DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Pwm7DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Pwm7DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Pwm7DutyCycle"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm7DutyCycle"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Pwm7DutyCycle"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pwm7DutyCycle"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Pwm7DutyCycle register.
    /// </summary>
    /// <seealso cref="Pwm7DutyCycle"/>
    [Description("Filters and selects timestamped messages from the Pwm7DutyCycle register.")]
    public partial class TimestampedPwm7DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Pwm7DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = Pwm7DutyCycle.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Pwm7DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Pwm7DutyCycle.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents an operator which creates standard message payloads for the
    /// CuttlefishCamTrigger device.
    /// </summary>
    /// <seealso cref="CreatePwmEnabledPayload"/>
    /// <seealso cref="CreatePwmSetPayload"/>
    /// <seealso cref="CreatePwmClearPayload"/>
    /// <seealso cref="CreatePwmInvertPayload"/>
    /// <seealso cref="CreateRisingEdgeEventEnabledPayload"/>
    /// <seealso cref="CreateRisingEdgeEventPayload"/>
    /// <seealso cref="CreatePwm0FrequencyHzPayload"/>
    /// <seealso cref="CreatePwm0DutyCyclePayload"/>
    /// <seealso cref="CreatePwm1FrequencyHzPayload"/>
    /// <seealso cref="CreatePwm1DutyCyclePayload"/>
    /// <seealso cref="CreatePwm2FrequencyHzPayload"/>
    /// <seealso cref="CreatePwm2DutyCyclePayload"/>
    /// <seealso cref="CreatePwm3FrequencyHzPayload"/>
    /// <seealso cref="CreatePwm3DutyCyclePayload"/>
    /// <seealso cref="CreatePwm4FrequencyHzPayload"/>
    /// <seealso cref="CreatePwm4DutyCyclePayload"/>
    /// <seealso cref="CreatePwm5FrequencyHzPayload"/>
    /// <seealso cref="CreatePwm5DutyCyclePayload"/>
    /// <seealso cref="CreatePwm6FrequencyHzPayload"/>
    /// <seealso cref="CreatePwm6DutyCyclePayload"/>
    /// <seealso cref="CreatePwm7FrequencyHzPayload"/>
    /// <seealso cref="CreatePwm7DutyCyclePayload"/>
    [XmlInclude(typeof(CreatePwmEnabledPayload))]
    [XmlInclude(typeof(CreatePwmSetPayload))]
    [XmlInclude(typeof(CreatePwmClearPayload))]
    [XmlInclude(typeof(CreatePwmInvertPayload))]
    [XmlInclude(typeof(CreateRisingEdgeEventEnabledPayload))]
    [XmlInclude(typeof(CreateRisingEdgeEventPayload))]
    [XmlInclude(typeof(CreatePwm0FrequencyHzPayload))]
    [XmlInclude(typeof(CreatePwm0DutyCyclePayload))]
    [XmlInclude(typeof(CreatePwm1FrequencyHzPayload))]
    [XmlInclude(typeof(CreatePwm1DutyCyclePayload))]
    [XmlInclude(typeof(CreatePwm2FrequencyHzPayload))]
    [XmlInclude(typeof(CreatePwm2DutyCyclePayload))]
    [XmlInclude(typeof(CreatePwm3FrequencyHzPayload))]
    [XmlInclude(typeof(CreatePwm3DutyCyclePayload))]
    [XmlInclude(typeof(CreatePwm4FrequencyHzPayload))]
    [XmlInclude(typeof(CreatePwm4DutyCyclePayload))]
    [XmlInclude(typeof(CreatePwm5FrequencyHzPayload))]
    [XmlInclude(typeof(CreatePwm5DutyCyclePayload))]
    [XmlInclude(typeof(CreatePwm6FrequencyHzPayload))]
    [XmlInclude(typeof(CreatePwm6DutyCyclePayload))]
    [XmlInclude(typeof(CreatePwm7FrequencyHzPayload))]
    [XmlInclude(typeof(CreatePwm7DutyCyclePayload))]
    [XmlInclude(typeof(CreateTimestampedPwmEnabledPayload))]
    [XmlInclude(typeof(CreateTimestampedPwmSetPayload))]
    [XmlInclude(typeof(CreateTimestampedPwmClearPayload))]
    [XmlInclude(typeof(CreateTimestampedPwmInvertPayload))]
    [XmlInclude(typeof(CreateTimestampedRisingEdgeEventEnabledPayload))]
    [XmlInclude(typeof(CreateTimestampedRisingEdgeEventPayload))]
    [XmlInclude(typeof(CreateTimestampedPwm0FrequencyHzPayload))]
    [XmlInclude(typeof(CreateTimestampedPwm0DutyCyclePayload))]
    [XmlInclude(typeof(CreateTimestampedPwm1FrequencyHzPayload))]
    [XmlInclude(typeof(CreateTimestampedPwm1DutyCyclePayload))]
    [XmlInclude(typeof(CreateTimestampedPwm2FrequencyHzPayload))]
    [XmlInclude(typeof(CreateTimestampedPwm2DutyCyclePayload))]
    [XmlInclude(typeof(CreateTimestampedPwm3FrequencyHzPayload))]
    [XmlInclude(typeof(CreateTimestampedPwm3DutyCyclePayload))]
    [XmlInclude(typeof(CreateTimestampedPwm4FrequencyHzPayload))]
    [XmlInclude(typeof(CreateTimestampedPwm4DutyCyclePayload))]
    [XmlInclude(typeof(CreateTimestampedPwm5FrequencyHzPayload))]
    [XmlInclude(typeof(CreateTimestampedPwm5DutyCyclePayload))]
    [XmlInclude(typeof(CreateTimestampedPwm6FrequencyHzPayload))]
    [XmlInclude(typeof(CreateTimestampedPwm6DutyCyclePayload))]
    [XmlInclude(typeof(CreateTimestampedPwm7FrequencyHzPayload))]
    [XmlInclude(typeof(CreateTimestampedPwm7DutyCyclePayload))]
    [Description("Creates standard message payloads for the CuttlefishCamTrigger device.")]
    public partial class CreateMessage : CreateMessageBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMessage"/> class.
        /// </summary>
        public CreateMessage()
        {
            Payload = new CreatePwmEnabledPayload();
        }

        string INamedElement.Name => $"{nameof(CuttlefishCamTrigger)}.{GetElementDisplayName(Payload)}";
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that bitmask to enable/disable each of the 8 Pwm outputs.
    /// </summary>
    [DisplayName("PwmEnabledPayload")]
    [Description("Creates a message payload that bitmask to enable/disable each of the 8 Pwm outputs.")]
    public partial class CreatePwmEnabledPayload
    {
        /// <summary>
        /// Gets or sets the value that bitmask to enable/disable each of the 8 Pwm outputs.
        /// </summary>
        [Description("The value that bitmask to enable/disable each of the 8 Pwm outputs.")]
        public Ports PwmEnabled { get; set; }

        /// <summary>
        /// Creates a message payload for the PwmEnabled register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public Ports GetPayload()
        {
            return PwmEnabled;
        }

        /// <summary>
        /// Creates a message that bitmask to enable/disable each of the 8 Pwm outputs.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the PwmEnabled register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.PwmEnabled.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that bitmask to enable/disable each of the 8 Pwm outputs.
    /// </summary>
    [DisplayName("TimestampedPwmEnabledPayload")]
    [Description("Creates a timestamped message payload that bitmask to enable/disable each of the 8 Pwm outputs.")]
    public partial class CreateTimestampedPwmEnabledPayload : CreatePwmEnabledPayload
    {
        /// <summary>
        /// Creates a timestamped message that bitmask to enable/disable each of the 8 Pwm outputs.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the PwmEnabled register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.PwmEnabled.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that bitmask to enable any of the 8 Pwm outputs when corresponding bit is set to 1.
    /// </summary>
    [DisplayName("PwmSetPayload")]
    [Description("Creates a message payload that bitmask to enable any of the 8 Pwm outputs when corresponding bit is set to 1.")]
    public partial class CreatePwmSetPayload
    {
        /// <summary>
        /// Gets or sets the value that bitmask to enable any of the 8 Pwm outputs when corresponding bit is set to 1.
        /// </summary>
        [Description("The value that bitmask to enable any of the 8 Pwm outputs when corresponding bit is set to 1.")]
        public Ports PwmSet { get; set; }

        /// <summary>
        /// Creates a message payload for the PwmSet register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public Ports GetPayload()
        {
            return PwmSet;
        }

        /// <summary>
        /// Creates a message that bitmask to enable any of the 8 Pwm outputs when corresponding bit is set to 1.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the PwmSet register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.PwmSet.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that bitmask to enable any of the 8 Pwm outputs when corresponding bit is set to 1.
    /// </summary>
    [DisplayName("TimestampedPwmSetPayload")]
    [Description("Creates a timestamped message payload that bitmask to enable any of the 8 Pwm outputs when corresponding bit is set to 1.")]
    public partial class CreateTimestampedPwmSetPayload : CreatePwmSetPayload
    {
        /// <summary>
        /// Creates a timestamped message that bitmask to enable any of the 8 Pwm outputs when corresponding bit is set to 1.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the PwmSet register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.PwmSet.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that bitmask to disable any of the 8 Pwm outputs when corresponding bit is set to 1.
    /// </summary>
    [DisplayName("PwmClearPayload")]
    [Description("Creates a message payload that bitmask to disable any of the 8 Pwm outputs when corresponding bit is set to 1.")]
    public partial class CreatePwmClearPayload
    {
        /// <summary>
        /// Gets or sets the value that bitmask to disable any of the 8 Pwm outputs when corresponding bit is set to 1.
        /// </summary>
        [Description("The value that bitmask to disable any of the 8 Pwm outputs when corresponding bit is set to 1.")]
        public Ports PwmClear { get; set; }

        /// <summary>
        /// Creates a message payload for the PwmClear register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public Ports GetPayload()
        {
            return PwmClear;
        }

        /// <summary>
        /// Creates a message that bitmask to disable any of the 8 Pwm outputs when corresponding bit is set to 1.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the PwmClear register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.PwmClear.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that bitmask to disable any of the 8 Pwm outputs when corresponding bit is set to 1.
    /// </summary>
    [DisplayName("TimestampedPwmClearPayload")]
    [Description("Creates a timestamped message payload that bitmask to disable any of the 8 Pwm outputs when corresponding bit is set to 1.")]
    public partial class CreateTimestampedPwmClearPayload : CreatePwmClearPayload
    {
        /// <summary>
        /// Creates a timestamped message that bitmask to disable any of the 8 Pwm outputs when corresponding bit is set to 1.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the PwmClear register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.PwmClear.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that bitmask to invert each of the 8 Pwm outputs if set to 1.
    /// </summary>
    [DisplayName("PwmInvertPayload")]
    [Description("Creates a message payload that bitmask to invert each of the 8 Pwm outputs if set to 1.")]
    public partial class CreatePwmInvertPayload
    {
        /// <summary>
        /// Gets or sets the value that bitmask to invert each of the 8 Pwm outputs if set to 1.
        /// </summary>
        [Description("The value that bitmask to invert each of the 8 Pwm outputs if set to 1.")]
        public Ports PwmInvert { get; set; }

        /// <summary>
        /// Creates a message payload for the PwmInvert register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public Ports GetPayload()
        {
            return PwmInvert;
        }

        /// <summary>
        /// Creates a message that bitmask to invert each of the 8 Pwm outputs if set to 1.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the PwmInvert register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.PwmInvert.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that bitmask to invert each of the 8 Pwm outputs if set to 1.
    /// </summary>
    [DisplayName("TimestampedPwmInvertPayload")]
    [Description("Creates a timestamped message payload that bitmask to invert each of the 8 Pwm outputs if set to 1.")]
    public partial class CreateTimestampedPwmInvertPayload : CreatePwmInvertPayload
    {
        /// <summary>
        /// Creates a timestamped message that bitmask to invert each of the 8 Pwm outputs if set to 1.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the PwmInvert register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.PwmInvert.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that bitmask to enable/disable dispatch of a rising edge event message for each of the corresponding Pwm outputs.
    /// </summary>
    [DisplayName("RisingEdgeEventEnabledPayload")]
    [Description("Creates a message payload that bitmask to enable/disable dispatch of a rising edge event message for each of the corresponding Pwm outputs.")]
    public partial class CreateRisingEdgeEventEnabledPayload
    {
        /// <summary>
        /// Gets or sets the value that bitmask to enable/disable dispatch of a rising edge event message for each of the corresponding Pwm outputs.
        /// </summary>
        [Description("The value that bitmask to enable/disable dispatch of a rising edge event message for each of the corresponding Pwm outputs.")]
        public byte RisingEdgeEventEnabled { get; set; }

        /// <summary>
        /// Creates a message payload for the RisingEdgeEventEnabled register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public byte GetPayload()
        {
            return RisingEdgeEventEnabled;
        }

        /// <summary>
        /// Creates a message that bitmask to enable/disable dispatch of a rising edge event message for each of the corresponding Pwm outputs.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the RisingEdgeEventEnabled register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.RisingEdgeEventEnabled.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that bitmask to enable/disable dispatch of a rising edge event message for each of the corresponding Pwm outputs.
    /// </summary>
    [DisplayName("TimestampedRisingEdgeEventEnabledPayload")]
    [Description("Creates a timestamped message payload that bitmask to enable/disable dispatch of a rising edge event message for each of the corresponding Pwm outputs.")]
    public partial class CreateTimestampedRisingEdgeEventEnabledPayload : CreateRisingEdgeEventEnabledPayload
    {
        /// <summary>
        /// Creates a timestamped message that bitmask to enable/disable dispatch of a rising edge event message for each of the corresponding Pwm outputs.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the RisingEdgeEventEnabled register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.RisingEdgeEventEnabled.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that bitmask with the current state of the Pwm outputs. This event is dispatched if any of the specified outputs sees a rising edge.
    /// </summary>
    [DisplayName("RisingEdgeEventPayload")]
    [Description("Creates a message payload that bitmask with the current state of the Pwm outputs. This event is dispatched if any of the specified outputs sees a rising edge.")]
    public partial class CreateRisingEdgeEventPayload
    {
        /// <summary>
        /// Gets or sets the value that bitmask with the current state of the Pwm outputs. This event is dispatched if any of the specified outputs sees a rising edge.
        /// </summary>
        [Description("The value that bitmask with the current state of the Pwm outputs. This event is dispatched if any of the specified outputs sees a rising edge.")]
        public byte RisingEdgeEvent { get; set; }

        /// <summary>
        /// Creates a message payload for the RisingEdgeEvent register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public byte GetPayload()
        {
            return RisingEdgeEvent;
        }

        /// <summary>
        /// Creates a message that bitmask with the current state of the Pwm outputs. This event is dispatched if any of the specified outputs sees a rising edge.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the RisingEdgeEvent register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.RisingEdgeEvent.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that bitmask with the current state of the Pwm outputs. This event is dispatched if any of the specified outputs sees a rising edge.
    /// </summary>
    [DisplayName("TimestampedRisingEdgeEventPayload")]
    [Description("Creates a timestamped message payload that bitmask with the current state of the Pwm outputs. This event is dispatched if any of the specified outputs sees a rising edge.")]
    public partial class CreateTimestampedRisingEdgeEventPayload : CreateRisingEdgeEventPayload
    {
        /// <summary>
        /// Creates a timestamped message that bitmask with the current state of the Pwm outputs. This event is dispatched if any of the specified outputs sees a rising edge.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the RisingEdgeEvent register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.RisingEdgeEvent.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pwm output 0 frequency setting in Hz.
    /// </summary>
    [DisplayName("Pwm0FrequencyHzPayload")]
    [Description("Creates a message payload that pwm output 0 frequency setting in Hz.")]
    public partial class CreatePwm0FrequencyHzPayload
    {
        /// <summary>
        /// Gets or sets the value that pwm output 0 frequency setting in Hz.
        /// </summary>
        [Description("The value that pwm output 0 frequency setting in Hz.")]
        public float Pwm0FrequencyHz { get; set; }

        /// <summary>
        /// Creates a message payload for the Pwm0FrequencyHz register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Pwm0FrequencyHz;
        }

        /// <summary>
        /// Creates a message that pwm output 0 frequency setting in Hz.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Pwm0FrequencyHz register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm0FrequencyHz.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pwm output 0 frequency setting in Hz.
    /// </summary>
    [DisplayName("TimestampedPwm0FrequencyHzPayload")]
    [Description("Creates a timestamped message payload that pwm output 0 frequency setting in Hz.")]
    public partial class CreateTimestampedPwm0FrequencyHzPayload : CreatePwm0FrequencyHzPayload
    {
        /// <summary>
        /// Creates a timestamped message that pwm output 0 frequency setting in Hz.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Pwm0FrequencyHz register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm0FrequencyHz.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pwm output 0 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [DisplayName("Pwm0DutyCyclePayload")]
    [Description("Creates a message payload that pwm output 0 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class CreatePwm0DutyCyclePayload
    {
        /// <summary>
        /// Gets or sets the value that pwm output 0 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        [Description("The value that pwm output 0 duty cycle setting (range: 0.0 - 1.0).")]
        public float Pwm0DutyCycle { get; set; }

        /// <summary>
        /// Creates a message payload for the Pwm0DutyCycle register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Pwm0DutyCycle;
        }

        /// <summary>
        /// Creates a message that pwm output 0 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Pwm0DutyCycle register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm0DutyCycle.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pwm output 0 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [DisplayName("TimestampedPwm0DutyCyclePayload")]
    [Description("Creates a timestamped message payload that pwm output 0 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class CreateTimestampedPwm0DutyCyclePayload : CreatePwm0DutyCyclePayload
    {
        /// <summary>
        /// Creates a timestamped message that pwm output 0 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Pwm0DutyCycle register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm0DutyCycle.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pwm output 1 frequency setting in Hz.
    /// </summary>
    [DisplayName("Pwm1FrequencyHzPayload")]
    [Description("Creates a message payload that pwm output 1 frequency setting in Hz.")]
    public partial class CreatePwm1FrequencyHzPayload
    {
        /// <summary>
        /// Gets or sets the value that pwm output 1 frequency setting in Hz.
        /// </summary>
        [Description("The value that pwm output 1 frequency setting in Hz.")]
        public float Pwm1FrequencyHz { get; set; }

        /// <summary>
        /// Creates a message payload for the Pwm1FrequencyHz register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Pwm1FrequencyHz;
        }

        /// <summary>
        /// Creates a message that pwm output 1 frequency setting in Hz.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Pwm1FrequencyHz register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm1FrequencyHz.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pwm output 1 frequency setting in Hz.
    /// </summary>
    [DisplayName("TimestampedPwm1FrequencyHzPayload")]
    [Description("Creates a timestamped message payload that pwm output 1 frequency setting in Hz.")]
    public partial class CreateTimestampedPwm1FrequencyHzPayload : CreatePwm1FrequencyHzPayload
    {
        /// <summary>
        /// Creates a timestamped message that pwm output 1 frequency setting in Hz.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Pwm1FrequencyHz register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm1FrequencyHz.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pwm output 1 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [DisplayName("Pwm1DutyCyclePayload")]
    [Description("Creates a message payload that pwm output 1 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class CreatePwm1DutyCyclePayload
    {
        /// <summary>
        /// Gets or sets the value that pwm output 1 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        [Description("The value that pwm output 1 duty cycle setting (range: 0.0 - 1.0).")]
        public float Pwm1DutyCycle { get; set; }

        /// <summary>
        /// Creates a message payload for the Pwm1DutyCycle register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Pwm1DutyCycle;
        }

        /// <summary>
        /// Creates a message that pwm output 1 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Pwm1DutyCycle register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm1DutyCycle.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pwm output 1 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [DisplayName("TimestampedPwm1DutyCyclePayload")]
    [Description("Creates a timestamped message payload that pwm output 1 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class CreateTimestampedPwm1DutyCyclePayload : CreatePwm1DutyCyclePayload
    {
        /// <summary>
        /// Creates a timestamped message that pwm output 1 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Pwm1DutyCycle register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm1DutyCycle.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pwm output 2 frequency setting in Hz.
    /// </summary>
    [DisplayName("Pwm2FrequencyHzPayload")]
    [Description("Creates a message payload that pwm output 2 frequency setting in Hz.")]
    public partial class CreatePwm2FrequencyHzPayload
    {
        /// <summary>
        /// Gets or sets the value that pwm output 2 frequency setting in Hz.
        /// </summary>
        [Description("The value that pwm output 2 frequency setting in Hz.")]
        public float Pwm2FrequencyHz { get; set; }

        /// <summary>
        /// Creates a message payload for the Pwm2FrequencyHz register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Pwm2FrequencyHz;
        }

        /// <summary>
        /// Creates a message that pwm output 2 frequency setting in Hz.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Pwm2FrequencyHz register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm2FrequencyHz.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pwm output 2 frequency setting in Hz.
    /// </summary>
    [DisplayName("TimestampedPwm2FrequencyHzPayload")]
    [Description("Creates a timestamped message payload that pwm output 2 frequency setting in Hz.")]
    public partial class CreateTimestampedPwm2FrequencyHzPayload : CreatePwm2FrequencyHzPayload
    {
        /// <summary>
        /// Creates a timestamped message that pwm output 2 frequency setting in Hz.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Pwm2FrequencyHz register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm2FrequencyHz.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pwm output 2 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [DisplayName("Pwm2DutyCyclePayload")]
    [Description("Creates a message payload that pwm output 2 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class CreatePwm2DutyCyclePayload
    {
        /// <summary>
        /// Gets or sets the value that pwm output 2 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        [Description("The value that pwm output 2 duty cycle setting (range: 0.0 - 1.0).")]
        public float Pwm2DutyCycle { get; set; }

        /// <summary>
        /// Creates a message payload for the Pwm2DutyCycle register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Pwm2DutyCycle;
        }

        /// <summary>
        /// Creates a message that pwm output 2 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Pwm2DutyCycle register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm2DutyCycle.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pwm output 2 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [DisplayName("TimestampedPwm2DutyCyclePayload")]
    [Description("Creates a timestamped message payload that pwm output 2 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class CreateTimestampedPwm2DutyCyclePayload : CreatePwm2DutyCyclePayload
    {
        /// <summary>
        /// Creates a timestamped message that pwm output 2 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Pwm2DutyCycle register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm2DutyCycle.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pwm output 3 frequency setting in Hz.
    /// </summary>
    [DisplayName("Pwm3FrequencyHzPayload")]
    [Description("Creates a message payload that pwm output 3 frequency setting in Hz.")]
    public partial class CreatePwm3FrequencyHzPayload
    {
        /// <summary>
        /// Gets or sets the value that pwm output 3 frequency setting in Hz.
        /// </summary>
        [Description("The value that pwm output 3 frequency setting in Hz.")]
        public float Pwm3FrequencyHz { get; set; }

        /// <summary>
        /// Creates a message payload for the Pwm3FrequencyHz register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Pwm3FrequencyHz;
        }

        /// <summary>
        /// Creates a message that pwm output 3 frequency setting in Hz.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Pwm3FrequencyHz register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm3FrequencyHz.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pwm output 3 frequency setting in Hz.
    /// </summary>
    [DisplayName("TimestampedPwm3FrequencyHzPayload")]
    [Description("Creates a timestamped message payload that pwm output 3 frequency setting in Hz.")]
    public partial class CreateTimestampedPwm3FrequencyHzPayload : CreatePwm3FrequencyHzPayload
    {
        /// <summary>
        /// Creates a timestamped message that pwm output 3 frequency setting in Hz.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Pwm3FrequencyHz register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm3FrequencyHz.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pwm output 3 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [DisplayName("Pwm3DutyCyclePayload")]
    [Description("Creates a message payload that pwm output 3 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class CreatePwm3DutyCyclePayload
    {
        /// <summary>
        /// Gets or sets the value that pwm output 3 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        [Description("The value that pwm output 3 duty cycle setting (range: 0.0 - 1.0).")]
        public float Pwm3DutyCycle { get; set; }

        /// <summary>
        /// Creates a message payload for the Pwm3DutyCycle register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Pwm3DutyCycle;
        }

        /// <summary>
        /// Creates a message that pwm output 3 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Pwm3DutyCycle register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm3DutyCycle.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pwm output 3 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [DisplayName("TimestampedPwm3DutyCyclePayload")]
    [Description("Creates a timestamped message payload that pwm output 3 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class CreateTimestampedPwm3DutyCyclePayload : CreatePwm3DutyCyclePayload
    {
        /// <summary>
        /// Creates a timestamped message that pwm output 3 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Pwm3DutyCycle register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm3DutyCycle.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pwm output 4 frequency setting in Hz.
    /// </summary>
    [DisplayName("Pwm4FrequencyHzPayload")]
    [Description("Creates a message payload that pwm output 4 frequency setting in Hz.")]
    public partial class CreatePwm4FrequencyHzPayload
    {
        /// <summary>
        /// Gets or sets the value that pwm output 4 frequency setting in Hz.
        /// </summary>
        [Description("The value that pwm output 4 frequency setting in Hz.")]
        public float Pwm4FrequencyHz { get; set; }

        /// <summary>
        /// Creates a message payload for the Pwm4FrequencyHz register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Pwm4FrequencyHz;
        }

        /// <summary>
        /// Creates a message that pwm output 4 frequency setting in Hz.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Pwm4FrequencyHz register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm4FrequencyHz.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pwm output 4 frequency setting in Hz.
    /// </summary>
    [DisplayName("TimestampedPwm4FrequencyHzPayload")]
    [Description("Creates a timestamped message payload that pwm output 4 frequency setting in Hz.")]
    public partial class CreateTimestampedPwm4FrequencyHzPayload : CreatePwm4FrequencyHzPayload
    {
        /// <summary>
        /// Creates a timestamped message that pwm output 4 frequency setting in Hz.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Pwm4FrequencyHz register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm4FrequencyHz.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pwm output 4 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [DisplayName("Pwm4DutyCyclePayload")]
    [Description("Creates a message payload that pwm output 4 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class CreatePwm4DutyCyclePayload
    {
        /// <summary>
        /// Gets or sets the value that pwm output 4 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        [Description("The value that pwm output 4 duty cycle setting (range: 0.0 - 1.0).")]
        public float Pwm4DutyCycle { get; set; }

        /// <summary>
        /// Creates a message payload for the Pwm4DutyCycle register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Pwm4DutyCycle;
        }

        /// <summary>
        /// Creates a message that pwm output 4 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Pwm4DutyCycle register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm4DutyCycle.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pwm output 4 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [DisplayName("TimestampedPwm4DutyCyclePayload")]
    [Description("Creates a timestamped message payload that pwm output 4 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class CreateTimestampedPwm4DutyCyclePayload : CreatePwm4DutyCyclePayload
    {
        /// <summary>
        /// Creates a timestamped message that pwm output 4 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Pwm4DutyCycle register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm4DutyCycle.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pwm output 5 frequency setting in Hz.
    /// </summary>
    [DisplayName("Pwm5FrequencyHzPayload")]
    [Description("Creates a message payload that pwm output 5 frequency setting in Hz.")]
    public partial class CreatePwm5FrequencyHzPayload
    {
        /// <summary>
        /// Gets or sets the value that pwm output 5 frequency setting in Hz.
        /// </summary>
        [Description("The value that pwm output 5 frequency setting in Hz.")]
        public float Pwm5FrequencyHz { get; set; }

        /// <summary>
        /// Creates a message payload for the Pwm5FrequencyHz register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Pwm5FrequencyHz;
        }

        /// <summary>
        /// Creates a message that pwm output 5 frequency setting in Hz.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Pwm5FrequencyHz register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm5FrequencyHz.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pwm output 5 frequency setting in Hz.
    /// </summary>
    [DisplayName("TimestampedPwm5FrequencyHzPayload")]
    [Description("Creates a timestamped message payload that pwm output 5 frequency setting in Hz.")]
    public partial class CreateTimestampedPwm5FrequencyHzPayload : CreatePwm5FrequencyHzPayload
    {
        /// <summary>
        /// Creates a timestamped message that pwm output 5 frequency setting in Hz.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Pwm5FrequencyHz register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm5FrequencyHz.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pwm output 5 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [DisplayName("Pwm5DutyCyclePayload")]
    [Description("Creates a message payload that pwm output 5 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class CreatePwm5DutyCyclePayload
    {
        /// <summary>
        /// Gets or sets the value that pwm output 5 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        [Description("The value that pwm output 5 duty cycle setting (range: 0.0 - 1.0).")]
        public float Pwm5DutyCycle { get; set; }

        /// <summary>
        /// Creates a message payload for the Pwm5DutyCycle register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Pwm5DutyCycle;
        }

        /// <summary>
        /// Creates a message that pwm output 5 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Pwm5DutyCycle register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm5DutyCycle.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pwm output 5 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [DisplayName("TimestampedPwm5DutyCyclePayload")]
    [Description("Creates a timestamped message payload that pwm output 5 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class CreateTimestampedPwm5DutyCyclePayload : CreatePwm5DutyCyclePayload
    {
        /// <summary>
        /// Creates a timestamped message that pwm output 5 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Pwm5DutyCycle register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm5DutyCycle.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pwm output 6 frequency setting in Hz.
    /// </summary>
    [DisplayName("Pwm6FrequencyHzPayload")]
    [Description("Creates a message payload that pwm output 6 frequency setting in Hz.")]
    public partial class CreatePwm6FrequencyHzPayload
    {
        /// <summary>
        /// Gets or sets the value that pwm output 6 frequency setting in Hz.
        /// </summary>
        [Description("The value that pwm output 6 frequency setting in Hz.")]
        public float Pwm6FrequencyHz { get; set; }

        /// <summary>
        /// Creates a message payload for the Pwm6FrequencyHz register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Pwm6FrequencyHz;
        }

        /// <summary>
        /// Creates a message that pwm output 6 frequency setting in Hz.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Pwm6FrequencyHz register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm6FrequencyHz.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pwm output 6 frequency setting in Hz.
    /// </summary>
    [DisplayName("TimestampedPwm6FrequencyHzPayload")]
    [Description("Creates a timestamped message payload that pwm output 6 frequency setting in Hz.")]
    public partial class CreateTimestampedPwm6FrequencyHzPayload : CreatePwm6FrequencyHzPayload
    {
        /// <summary>
        /// Creates a timestamped message that pwm output 6 frequency setting in Hz.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Pwm6FrequencyHz register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm6FrequencyHz.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pwm output 6 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [DisplayName("Pwm6DutyCyclePayload")]
    [Description("Creates a message payload that pwm output 6 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class CreatePwm6DutyCyclePayload
    {
        /// <summary>
        /// Gets or sets the value that pwm output 6 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        [Description("The value that pwm output 6 duty cycle setting (range: 0.0 - 1.0).")]
        public float Pwm6DutyCycle { get; set; }

        /// <summary>
        /// Creates a message payload for the Pwm6DutyCycle register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Pwm6DutyCycle;
        }

        /// <summary>
        /// Creates a message that pwm output 6 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Pwm6DutyCycle register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm6DutyCycle.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pwm output 6 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [DisplayName("TimestampedPwm6DutyCyclePayload")]
    [Description("Creates a timestamped message payload that pwm output 6 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class CreateTimestampedPwm6DutyCyclePayload : CreatePwm6DutyCyclePayload
    {
        /// <summary>
        /// Creates a timestamped message that pwm output 6 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Pwm6DutyCycle register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm6DutyCycle.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pwm output 7 frequency setting in Hz.
    /// </summary>
    [DisplayName("Pwm7FrequencyHzPayload")]
    [Description("Creates a message payload that pwm output 7 frequency setting in Hz.")]
    public partial class CreatePwm7FrequencyHzPayload
    {
        /// <summary>
        /// Gets or sets the value that pwm output 7 frequency setting in Hz.
        /// </summary>
        [Description("The value that pwm output 7 frequency setting in Hz.")]
        public float Pwm7FrequencyHz { get; set; }

        /// <summary>
        /// Creates a message payload for the Pwm7FrequencyHz register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Pwm7FrequencyHz;
        }

        /// <summary>
        /// Creates a message that pwm output 7 frequency setting in Hz.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Pwm7FrequencyHz register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm7FrequencyHz.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pwm output 7 frequency setting in Hz.
    /// </summary>
    [DisplayName("TimestampedPwm7FrequencyHzPayload")]
    [Description("Creates a timestamped message payload that pwm output 7 frequency setting in Hz.")]
    public partial class CreateTimestampedPwm7FrequencyHzPayload : CreatePwm7FrequencyHzPayload
    {
        /// <summary>
        /// Creates a timestamped message that pwm output 7 frequency setting in Hz.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Pwm7FrequencyHz register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm7FrequencyHz.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pwm output 7 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [DisplayName("Pwm7DutyCyclePayload")]
    [Description("Creates a message payload that pwm output 7 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class CreatePwm7DutyCyclePayload
    {
        /// <summary>
        /// Gets or sets the value that pwm output 7 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        [Description("The value that pwm output 7 duty cycle setting (range: 0.0 - 1.0).")]
        public float Pwm7DutyCycle { get; set; }

        /// <summary>
        /// Creates a message payload for the Pwm7DutyCycle register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Pwm7DutyCycle;
        }

        /// <summary>
        /// Creates a message that pwm output 7 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Pwm7DutyCycle register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm7DutyCycle.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pwm output 7 duty cycle setting (range: 0.0 - 1.0).
    /// </summary>
    [DisplayName("TimestampedPwm7DutyCyclePayload")]
    [Description("Creates a timestamped message payload that pwm output 7 duty cycle setting (range: 0.0 - 1.0).")]
    public partial class CreateTimestampedPwm7DutyCyclePayload : CreatePwm7DutyCyclePayload
    {
        /// <summary>
        /// Creates a timestamped message that pwm output 7 duty cycle setting (range: 0.0 - 1.0).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Pwm7DutyCycle register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return AllenNeuralDynamics.CuttlefishCamTrigger.Pwm7DutyCycle.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Available ports on the device
    /// </summary>
    [Flags]
    public enum Ports : byte
    {
        None = 0x0,
        Port0 = 0x1,
        Port1 = 0x2,
        Port2 = 0x4,
        Port3 = 0x8,
        Port4 = 0x10,
        Port5 = 0x20,
        Port6 = 0x40,
        Port7 = 0x80
    }
}
