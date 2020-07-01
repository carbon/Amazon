using Xunit;

namespace Amazon.Ec2.Tests
{
    public class DescribeInstancesTypesResponseTests
    {
        [Fact]
        public void Deserialize()
        {
            string xml = @"<DescribeInstanceTypesResponse xmlns=""http://ec2.amazonaws.com/doc/2016-11-15/"">
        <requestId>a</requestId>
        <instanceTypeSet>
            <item>
                <autoRecoverySupported>false</autoRecoverySupported>
                <bareMetal>false</bareMetal>
                <burstablePerformanceSupported>false</burstablePerformanceSupported>
                <currentGeneration>false</currentGeneration>
                <dedicatedHostsSupported>false</dedicatedHostsSupported>
                <ebsInfo>
                    <ebsOptimizedSupport>unsupported</ebsOptimizedSupport>
                    <encryptionSupport>unsupported</encryptionSupport>
                </ebsInfo>
                <freeTierEligible>true</freeTierEligible>
                <hibernationSupported>false</hibernationSupported>
                <hypervisor>xen</hypervisor>
                <instanceStorageSupported>false</instanceStorageSupported>
                <instanceType>t1.micro</instanceType>
                <memoryInfo>
                    <sizeInMiB>627</sizeInMiB>
                </memoryInfo>
                <networkInfo>
                    <enaSupport>unsupported</enaSupport>
                    <ipv4AddressesPerInterface>2</ipv4AddressesPerInterface>
                    <ipv6AddressesPerInterface>0</ipv6AddressesPerInterface>
                    <ipv6Supported>false</ipv6Supported>
                    <maximumNetworkInterfaces>2</maximumNetworkInterfaces>
                    <networkPerformance>Very Low</networkPerformance>
                </networkInfo>
                <placementGroupInfo>
                    <supportedStrategies>
                        <item>partition</item>
                        <item>spread</item>
                    </supportedStrategies>
                </placementGroupInfo>
                <processorInfo>
                    <supportedArchitectures>
                        <item>i386</item>
                        <item>x86_64</item>
                    </supportedArchitectures>
                </processorInfo>
                <supportedRootDeviceTypes>
                    <item>ebs</item>
                </supportedRootDeviceTypes>
                <supportedUsageClasses>
                    <item>on-demand</item>
                </supportedUsageClasses>
                <vCpuInfo>
                    <defaultCores>1</defaultCores>
                    <defaultThreadsPerCore>1</defaultThreadsPerCore>
                    <defaultVCpus>1</defaultVCpus>
                    <validCores>
                        <item>1</item>
                    </validCores>
                    <validThreadsPerCore>
                        <item>1</item>
                    </validThreadsPerCore>
                </vCpuInfo>
            </item>
        </instanceTypeSet>
    </DescribeInstanceTypesResponse>";


            var response = DescribeInstanceTypesResponse.Deserialize(xml);

            Assert.Equal("a", response.RequestId);

            Assert.Single(response.InstanceTypes);

            var instance = response.InstanceTypes[0];

            Assert.Equal("t1.micro", instance.InstanceType);
            Assert.Equal("xen", instance.Hypervisor);

            Assert.True(instance.FreeTierEligible);

            Assert.Equal(new[] { "i386", "x86_64" }, instance.ProcessorInfo.SupportedArchitectures);
            Assert.Equal(new[] { "ebs" }, instance.SupportedRootDeviceTypes);
            Assert.Equal(new[] { "on-demand" }, instance.SupportedUsageClasses);

            // Memory
            Assert.Equal(627, instance.MemoryInfo.SizeInMiB);

            // Network
            Assert.Equal("unsupported", instance.NetworkInfo.EnaSupport);
            Assert.Equal(2, instance.NetworkInfo.Ipv4AddressesPerInterface);
            Assert.Equal(0, instance.NetworkInfo.Ipv6AddressesPerInterface);
            Assert.False(instance.NetworkInfo.Ipv6Supported);
            Assert.Equal(2, instance.NetworkInfo.MaximumNetworkInterfaces);
            Assert.Equal("Very Low", instance.NetworkInfo.NetworkPerformance);


            // vCPU
            Assert.Equal(1, instance.VCpuInfo.DefaultCores);
            Assert.Equal(1, instance.VCpuInfo.DefaultThreadsPerCore);
            Assert.Equal(1, instance.VCpuInfo.DefaultVCpus);
            Assert.Equal(new[] { 1 }, instance.VCpuInfo.ValidThreadsPerCore);

        }
    }
}
