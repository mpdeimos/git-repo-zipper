using System;
using System.Collections.Generic;

namespace Mpdeimos.GitRepoZipper
{
	public class ZipScenario
	{
		public string[] Sources { get; private set; }

		public Dictionary<string, string[]> Branches { get; } = new Dictionary<string, string[]>();

		public string[] this[string name] { set { this.Branches[name] = value; } }

		public ZipScenario(params string[] sources)
		{
			this.Sources = sources;
		}

		/// <summary>
		/// Different zip scenarios.
		/// </summary>
		public static ZipScenario[] Scenarios
		{
			get
			{ 
				return new [] { new ZipScenario(TestData.GitTwoSimpleBranchesA) { 
						["master" ] = new[] {
							"34fbde4c98cc29773cd3483aa44c26d5cca816f5",
							"9d46d63c3345bab7f9fafaa5fdf13f3c04bbe2b8",
						},
						["1" ] = new[] {
							"34fbde4c98cc29773cd3483aa44c26d5cca816f5",
							"9f9d6e3068f26f00b076b10380764a3519490486",
						}
					},
					new ZipScenario(TestData.GitTwoSimpleBranchesA, TestData.GitTwoSimpleBranchesB) { 
						["master" ] = new[] {
							"34fbde4c98cc29773cd3483aa44c26d5cca816f5",
							"9590d59b0d639f5cd8d1e2da03ae52161ec02b2b",
							"4168bb0842f025ddb7975ebbd9a9bd7081555a6a",
							"0033408fbb0366dbda1f6a86ecef041021378e8b",
							"9d46d63c3345bab7f9fafaa5fdf13f3c04bbe2b8",
						},
						["1" ] = new[] {
							"34fbde4c98cc29773cd3483aa44c26d5cca816f5",
							"9590d59b0d639f5cd8d1e2da03ae52161ec02b2b",
							"4168bb0842f025ddb7975ebbd9a9bd7081555a6a",
							"26074c825ec2a948d19ffd69e71b333ee5e62f70",
							"9f9d6e3068f26f00b076b10380764a3519490486",
							"c3eaaec75fb1dd7e1dcab7f8d2efe8bef83f7100",
						}
					},
					new ZipScenario(TestData.GitTwoSimpleBranchesA, TestData.GitTwoSimpleBranchesB, TestData.GitTwoSimpleBranchesC) { 
						["master" ] = new[] {
							"34fbde4c98cc29773cd3483aa44c26d5cca816f5",
							"9590d59b0d639f5cd8d1e2da03ae52161ec02b2b",
							"4168bb0842f025ddb7975ebbd9a9bd7081555a6a",
							"296eb9d44fa9e525b60d874cf1fa2c35c60b7668",
							"0033408fbb0366dbda1f6a86ecef041021378e8b",
							"9d46d63c3345bab7f9fafaa5fdf13f3c04bbe2b8",
							"fb3e1a686cbbbdbd633e0ef703602c836b578647",
						},
						["1" ] = new[] {
							"34fbde4c98cc29773cd3483aa44c26d5cca816f5",
							"9590d59b0d639f5cd8d1e2da03ae52161ec02b2b",
							"4168bb0842f025ddb7975ebbd9a9bd7081555a6a",
							"26074c825ec2a948d19ffd69e71b333ee5e62f70",
							"9f9d6e3068f26f00b076b10380764a3519490486",
							"c3eaaec75fb1dd7e1dcab7f8d2efe8bef83f7100",
						}
					}
				};
			}
		}
	}
}

