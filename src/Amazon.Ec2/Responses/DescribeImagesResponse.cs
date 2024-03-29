﻿#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class DescribeImagesResponse : IEc2Response
{
    [XmlArray("imagesSet")]
    [XmlArrayItem("item")]
    public Image[] Images { get; init; }
}

/*
<DescribeImagesResponse xmlns="http://ec2.amazonaws.com/doc/2016-11-15/">
  <requestId>59dbff89-35bd-4eac-99ed-be587EXAMPLE</requestId> 
  <imagesSet>
    <item>
      <imageId>ami-1a2b3c4d</imageId>
      <imageLocation>amazon/getting-started</imageLocation>
      <imageState>available</imageState>
      <imageOwnerId>123456789012</imageOwnerId>
      <isPublic>true</isPublic>
      <architecture>i386</architecture>
      <imageType>machine</imageType>
      <kernelId>aki-1a2b3c4d</kernelId>
      <ramdiskId>ari-1a2b3c4d</ramdiskId>
      <imageOwnerAlias>amazon</imageOwnerAlias>
      <name>getting-started</name>
      <description>Image Description</description>
      <rootDeviceType>ebs</rootDeviceType>
      <rootDeviceName>/dev/sda</rootDeviceName>
      <blockDeviceMapping>
        <item>
          <deviceName>/dev/sda1</deviceName>
          <ebs>
            <snapshotId>snap-1234567890abcdef0</snapshotId>
            <volumeSize>15</volumeSize>
            <deleteOnTermination>false</deleteOnTermination>
            <volumeType>standard</volumeType>
          </ebs>
        </item>
      </blockDeviceMapping>
      <virtualizationType>paravirtual</virtualizationType>
      <tagSet/>
      <hypervisor>xen</hypervisor>
    </item>
  </imagesSet>
</DescribeImagesResponse>
*/
