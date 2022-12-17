using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HW6db.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exchange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExchangeName = table.Column<string>(type: "text", nullable: false),
                    ExchangeSymbol = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exchange", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FutureEvaluation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    marketPrice = table.Column<double>(type: "double precision", nullable: false),
                    TradeId = table.Column<int>(type: "integer", nullable: false),
                    EvaluationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    pnl = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FutureEvaluation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OptionProperty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OptionType = table.Column<string>(type: "text", nullable: false),
                    IsCall = table.Column<bool>(type: "boolean", nullable: false),
                    PayOut = table.Column<double>(type: "double precision", nullable: false),
                    KnockType = table.Column<string>(type: "text", nullable: false),
                    BarrierLevel = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionProperty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RateCurve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Symbol = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateCurve", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RatePoint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RateCurveId = table.Column<int>(type: "integer", nullable: false),
                    Rate = table.Column<double>(type: "double precision", nullable: false),
                    Term = table.Column<double>(type: "double precision", nullable: false),
                    RecordDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatePoint", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tradeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    instrumentId = table.Column<int>(type: "integer", nullable: false),
                    instrumentType = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnderlyingEvaluation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    marketPrice = table.Column<double>(type: "double precision", nullable: false),
                    TradeId = table.Column<int>(type: "integer", nullable: false),
                    dateEvl = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    pnl = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnderlyingEvaluation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UnitType = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Market",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MarketName = table.Column<string>(type: "text", nullable: false),
                    ExchangeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Market", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Market_Exchange_ExchangeId",
                        column: x => x.ExchangeId,
                        principalTable: "Exchange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Instrument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Symbol = table.Column<string>(type: "text", nullable: false),
                    MarketId = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    UnitId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instrument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instrument_Market_MarketId",
                        column: x => x.MarketId,
                        principalTable: "Market",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Instrument_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Underlying",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Underlying", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Underlying_Instrument_Id",
                        column: x => x.Id,
                        principalTable: "Instrument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Future",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Expirationdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UnderlyingId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Future", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Future_Instrument_Id",
                        column: x => x.Id,
                        principalTable: "Instrument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Future_Underlying_UnderlyingId",
                        column: x => x.UnderlyingId,
                        principalTable: "Underlying",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Option",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    UnderlyingId = table.Column<int>(type: "integer", nullable: false),
                    Expirationdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OPId = table.Column<int>(type: "integer", nullable: false),
                    vol = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Option", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Option_Instrument_Id",
                        column: x => x.Id,
                        principalTable: "Instrument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Option_OptionProperty_OPId",
                        column: x => x.OPId,
                        principalTable: "OptionProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Option_Underlying_UnderlyingId",
                        column: x => x.UnderlyingId,
                        principalTable: "Underlying",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OptionEvaluation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optionId = table.Column<int>(type: "integer", nullable: false),
                    dateEvl = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TradeId = table.Column<int>(type: "integer", nullable: false),
                    simulatePrice = table.Column<double>(type: "double precision", nullable: false),
                    pnl = table.Column<double>(type: "double precision", nullable: false),
                    Delta = table.Column<double>(type: "double precision", nullable: false),
                    Theta = table.Column<double>(type: "double precision", nullable: false),
                    Gamma = table.Column<double>(type: "double precision", nullable: false),
                    Vega = table.Column<double>(type: "double precision", nullable: false),
                    Rho = table.Column<double>(type: "double precision", nullable: false),
                    StdErrorNorm = table.Column<double>(type: "double precision", nullable: false),
                    StdErrorReduce = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionEvaluation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionEvaluation_Option_optionId",
                        column: x => x.optionId,
                        principalTable: "Option",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Future_UnderlyingId",
                table: "Future",
                column: "UnderlyingId");

            migrationBuilder.CreateIndex(
                name: "IX_Instrument_MarketId",
                table: "Instrument",
                column: "MarketId");

            migrationBuilder.CreateIndex(
                name: "IX_Instrument_UnitId",
                table: "Instrument",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Market_ExchangeId",
                table: "Market",
                column: "ExchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_OPId",
                table: "Option",
                column: "OPId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_UnderlyingId",
                table: "Option",
                column: "UnderlyingId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionEvaluation_optionId",
                table: "OptionEvaluation",
                column: "optionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Future");

            migrationBuilder.DropTable(
                name: "FutureEvaluation");

            migrationBuilder.DropTable(
                name: "OptionEvaluation");

            migrationBuilder.DropTable(
                name: "RateCurve");

            migrationBuilder.DropTable(
                name: "RatePoint");

            migrationBuilder.DropTable(
                name: "Trade");

            migrationBuilder.DropTable(
                name: "UnderlyingEvaluation");

            migrationBuilder.DropTable(
                name: "Option");

            migrationBuilder.DropTable(
                name: "OptionProperty");

            migrationBuilder.DropTable(
                name: "Underlying");

            migrationBuilder.DropTable(
                name: "Instrument");

            migrationBuilder.DropTable(
                name: "Market");

            migrationBuilder.DropTable(
                name: "Unit");

            migrationBuilder.DropTable(
                name: "Exchange");
        }
    }
}
